// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Identity;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: <program> <storage-account-name> <container-name> <Workspace ID> <Connection ID>");
            return;
        }

        string accountName = args[0];
        string containerName = args[1];
        string workspaceId = args[2]; // Workspace ID (passed as an argument)
        string connectionId = args[3]; // Connection ID (passed as an argument)
        string dfsUri = $"https://{accountName}.dfs.core.windows.net";
        string folderToFind = "_delta_log"; // Folder name to search for

        // Use InteractiveBrowserCredential for interactive login via a web browser
        var storageCredential = new InteractiveBrowserCredential();
        var apiCredential = new InteractiveBrowserCredential();

        DataLakeServiceClient serviceClient = new DataLakeServiceClient(new Uri(dfsUri), storageCredential);
        DataLakeFileSystemClient fileSystemClient = serviceClient.GetFileSystemClient(containerName);

        Console.WriteLine("start");
        List<(string Path, string ParentFolder)> foundPaths = new List<(string Path, string ParentFolder)>();

        try
        {
            // Search for the specified folder in the container
            await SearchForDeltaLog(fileSystemClient, string.Empty, folderToFind, foundPaths);
        }
        catch (RequestFailedException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Print the found paths and their parent folders
        foreach (var (Path, ParentFolder) in foundPaths)
        {
            Console.WriteLine($"Path: {Path}, Parent Folder: {ParentFolder}");
            // Create Fabric shortcut for each found path
            string fullPath = $"{containerName}/{Path}";
            string subPath = fullPath.Replace("/_delta_log", ""); // Remove "_delta_log" from the subpath
            await CreateFabricShortcut(workspaceId, connectionId, fullPath, ParentFolder, apiCredential, dfsUri, subPath);
        }
    }

    // Recursive method to search for the specified folder in the container
    static async Task SearchForDeltaLog(DataLakeFileSystemClient fileSystemClient, string directoryPath, string folderToFind, List<(string Path, string ParentFolder)> foundPaths)
    {
        await foreach (PathItem pathItem in fileSystemClient.GetPathsAsync(directoryPath))
        {
            // Check if the current path item matches the folder name
            if (pathItem.Name.EndsWith(folderToFind))
            {
                // Extract the folder name one level above the pathItem.Name
                string parentFolder = GetParentFolder(pathItem.Name);
                foundPaths.Add((pathItem.Name, parentFolder));
            }

            // If the current path item is a directory, search recursively
            if (pathItem.IsDirectory == true)
            {
                await SearchForDeltaLog(fileSystemClient, pathItem.Name, folderToFind, foundPaths);
            }
        }
    }

    // Helper method to extract the parent folder name
    static string GetParentFolder(string path)
    {
        // Split the path by '/' and get the second last element
        var parts = path.Split('/');
        if (parts.Length > 1)
        {
            return parts[parts.Length - 2];
        }
        return string.Empty;
    }

    // Method to create a Fabric shortcut
    static async Task CreateFabricShortcut(string workspaceId, string connectionId, string targetPath, string shortcutName, TokenCredential credential, string dfsUri, string subPath)
    {
        using (HttpClient client = new HttpClient())
        {
            // Use InteractiveBrowserCredential to get the access token
            var tokenRequestContext = new TokenRequestContext(new[] { "https://api.fabric.microsoft.com/.default" });
            var token = await credential.GetTokenAsync(tokenRequestContext, System.Threading.CancellationToken.None);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);

            var shortcutPayload = new
            {
                path = "Tables",
                name = shortcutName,
                target = new
                {
                    type = "AdlsGen2",
                    adlsGen2 = new
                    {
                        location = dfsUri,
                        subpath = subPath,
                        connectionId = connectionId
                    }
                }
            };

            string jsonPayload = JsonConvert.SerializeObject(shortcutPayload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Ensure the endpoint URL is correct
            string endpointUrl = $"https://api.fabric.microsoft.com/v1/workspaces/{workspaceId}/items/6e2931b3-6c39-4dde-8b99-4c23fbd52eae/shortcuts";
            HttpResponseMessage response = await client.PostAsync(endpointUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Shortcut created successfully for path: {targetPath}");
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to create shortcut for path: {subPath}. Status Code: {response.StatusCode}, Response: {responseBody}");
            }
        }
    }
}
