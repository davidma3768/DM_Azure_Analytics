Accelerating Delta Lake Shortcut Creation in Nested Folders with Fabric API

As more organizations adopt Microsoft Fabric and seek to integrate Databricks-processed datasets, particularly Delta Lake tables, the need for seamless integration between these platforms has become essential. Fabric shortcuts play a crucial role in enabling this integration, allowing users to reuse Databricks-produced Delta Lake tables within Fabric. However, shortcuts come with certain limitations. This article provides a solution to efficiently address those limitations using the Fabric Shortcut API. 

This post addresses a specific challenge related to integrating unmanaged Delta Lake tables stored in Azure Data Lake Storage Gen2 (ADLSG2) with Microsoft Fabric. While future updates on Fabric and Databricks Unity Catalog integration are anticipated, this discussion focuses on a current solution for efficiently handling Delta Lake datasets using Fabric shortcuts.

Understanding Fabric Lakehouse Shortcuts
Fabric Lakehouse shortcuts are a powerful feature that allows users to reference data stored in various locations without the need to copy it. This capability unifies data from different Lakehouses, workspaces, or external storage types, such as Azure Data Lake Storage Gen2 (ADLS Gen2), Amazon S3, and Dataverse12. By creating shortcuts, you can quickly make large amounts of data available in your Lakehouse locally, eliminating the latency associated with copying data from the source.

Types of Shortcuts

Shortcuts in Microsoft Fabric can point to both internal and external storage locations. The location that a shortcut points to is known as the target path, while the location where the shortcut appears is known as the shortcut path. These shortcuts behave like symbolic links, allowing any workload or service with access to OneLake to use them2.

Creating Shortcuts for Delta Lake Tables

You can create shortcuts in Lakehouses and Kusto Query Language (KQL) databases. In Lakehouses, shortcuts can be created at the top level of the Tables folder or at any level within the Files folder. When a shortcut points to data in the Delta or Parquet format, the Lakehouse automatically synchronizes the metadata and recognizes the folder as a table2.

Access Control

Access control for shortcuts depends on the source. Shortcuts to Microsoft Fabric internal sources use the calling user’s identity, while external shortcuts use the connectivity details specified during creation1.

Schema-Level Shortcuts

Fabric also supports schema-level shortcuts (in public preview as of Aug 2024), which allow users to create shortcuts at a higher level in the folder hierarchy. This feature is particularly useful for organizing and managing large datasets across different schemas. However, this feature has limitation with complex folder structure, see details in next section.

By leveraging Fabric Lakehouse shortcuts, you can efficiently manage and access data from various sources, streamlining your data integration processes and enhancing productivity.

Implementation Challenge with Lakehouse Shortcut
In a real-world scenario, Delta Lake tables are often produced in various ways, adhering to a medallion architecture and project-specific or LOB structure. As a result, these datasets are typically stored in a complex, multi-level folder structure within Azure Data Lake Storage Gen2 (ADLSG2). With thousands of tables spread across different directories, containers, and even storage accounts, creating shortcuts via UI becomes a significant challenge.

Folder Detection Issues: If the "_delta_log" folder—a critical component for recognizing Delta Lake tables—is not detected at the current folder level, the files will be placed under an unidentified folder. This results in an invalid Delta Table format, making the data unusable as a table within Microsoft Fabric.

Manual Shortcut Creation: Given the scale, creating shortcuts one by one through UI is not feasible when there are large numbers of Delta Lake Tables in the storage account. Managing those manually is time-consuming and prone to errors.

Schema-Level Shortcut Limitations: While schema-level shortcuts in Fabric allow users to move one level up in the folder structure to create shortcuts for multiple tables in one shot, but it still cannot recognize Delta tables stored in deeper nested directories. Consequently, these tables are placed under unidentified folders, rendering them unusable as Delta Lake tables.

Microsift Fabric API
The Fabric API provides a comprehensive set of tools for interacting with Microsoft Fabric. It allows developers to programmatically manage and interact with various Fabric services, making it easier to integrate and automate workflows.Key Features of the Fabric APIResource Management: Create, update, and delete resources within Fabric.Data Operations: Perform CRUD (Create, Read, Update, Delete) operations on data stored in Fabric.Authentication: Securely authenticate and authorize access to Fabric resources.For more detailed information, you can refer to the official documentation here.Fabric Shortcut APIThe Fabric Shortcut API is a powerful feature that allows you to create shortcuts within OneLake, enabling seamless access to various data sources without the need to physically move or copy data. This can significantly reduce data duplication and improve efficiency.Key Features of the Fabric Shortcut APIUnified Data Access: Create shortcuts to unify data across different domains, clouds, and accounts.Simplified Permissions: Manage permissions and credentials centrally, eliminating the need for separate configurations for each data source.Reduced Latency: Minimize process latency by eliminating the need for data copies and staging.You can create shortcuts programmatically using the REST API, which supports operations such as creating, deleting, and listing shortcuts. For more details, check out the official documentationnbsp;here.

Automation with Fabric Shortcut API
To address this challenge, a C# script was developed to automate the process. The solution involves:

Scanning the Storage Account Container: The script recursively scans the storage container to identify all folders named "_delta_log," which indicates the presence of Delta Lake tables.

Generating a Path List: The script compiles a list of all identified Delta Lake table paths within the storage container.

Automated Shortcut Creation: Using the Microsoft Fabric API (API reference), the script automatically creates shortcuts under the Tables folder in the Fabric Lakehouse. This automation ensures that Delta Lake tables are correctly integrated and accessible within Fabric.

The function takes the following parameters:

Storage Account Name

Container Name

Workspace ID

Connection ID

Based on the example provided here, the script handles shortcut creation, with Fabric managing any format issues.

Implementation Steps:

For those interested in implementing this solution, here are the steps to get started with VS Code and C#:

Set Up Your Environment:

Create an Azure Function:

Install Required Packages:

Implement the Logic:

Deploy and Test:

This automated solution drastically reduces the manual effort required to manage Delta Lake tables in Fabric, making the process more efficient and scalable. The full code and detailed instructions will be shared in a subsequent blog post.

Stay tuned for more updates on Microsoft Fabric and Databricks integration!
