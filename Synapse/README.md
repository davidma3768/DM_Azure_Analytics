# Azure Synpase Knowledge Repositor (in construction)

## Objective


---

_To cover the following_

---

### Gen2 Services within Synapse: 
    -- Dedicated SQL Pool

    -- Lake Database / Serverless

    -- Spark Pool and Spark related features

    -- Pipeline

    -- Security related
  
  
 Gen3
 
 ### POC Framework for Synapse Analytics project
 
 ### DB and Table Design
 
 ### Performance Tuning and Troubleshooting
 
 
---

_Keeping below content for MD reference_

---
 
![Data Management & Analytics](/docs/images/DataManagementAnalytics.gif)


This repository describes a framework...

> **Note:** Before getting started with the deployment, please make sure you are familiar with the [complementary documentation in the Cloud Adoption Framework](https://aka.ms/adopt/datamanagement). Also, before deploying your first Data Product, please make sure that you have deployed a [Data Management Landing Zone](https://github.com/Azure/data-management-zone) and at least one [Data Landing Zone](https://github.com/Azure/data-landing-zone). The minimal recommended setup consists of a single [Data Management Landing Zone](https://github.com/Azure/data-management-zone) and a single [Data Landing Zone](https://github.com/Azure/data-landing-zone).

## Deploy Data Management & Analytics Scenario

The Data Management & Analytics architecture is modular by design and allows customers to start with a small footprint and grow over time. In order to not end up in a migration project, customers should decide upfront how they want to organize data domains across Data Landing Zones. All Data Management & Analytics architecture building blocks can be deployed through the Azure Portal as well as through GitHub Actions workflows and Azure DevOps Pipelines. The template repositories contain sample YAML pipelines to more quickly get started with the setup of the environments.

| Reference implementation   | Description | Deploy to Azure | Link |
|:---------------------------|:------------|:----------------|------|
| Data Management & Analytics Scenario | Deploys a [Data Management Landing Zone](https://github.com/Azure/data-management-zone) and one or multiple Data Landing Zones all at once. Provides less options than the the individual Data Management Landing Zone and Data Landing Zone deployment options. Helps you to quickly get started and make yourself familiar with the reference design. For more advanced scenarios, please deploy the artifacts individually. |[![Deploy To Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#blade/Microsoft_Azure_CreateUIDef/CustomDeploymentBlade/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-management-zone%2Fmain%2Fdocs%2Freference%2FdataManagementAnalytics.json/uiFormDefinitionUri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-management-zone%2Fmain%2Fdocs%2Freference%2Fportal.dataManagementAnalytics.json) |  |
| Data Management Landing Zone       | Deploys a single Data Management Landing Zone to a subscription. |[![Deploy To Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#blade/Microsoft_Azure_CreateUIDef/CustomDeploymentBlade/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-management-zone%2Fmain%2Finfra%2Fmain.json/uiFormDefinitionUri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-management-zone%2Fmain%2Fdocs%2Freference%2Fportal.dataManagementZone.json) | [Repository](https://github.com/Azure/data-management-zone) |
| Data Landing Zone          | Deploys a single Data Landing Zone to a subscription. Please deploy a [Data Management Landing Zone](https://github.com/Azure/data-management-zone) first. |[![Deploy To Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#blade/Microsoft_Azure_CreateUIDef/CustomDeploymentBlade/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-landing-zone%2Fmain%2Finfra%2Fmain.json/uiFormDefinitionUri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-landing-zone%2Fmain%2Fdocs%2Freference%2Fportal.dataLandingZone.json) | [Repository](https://github.com/Azure/data-landing-zone) |
| Data Product Batch     | Deploys a Data Workload template for Data Batch Analysis to a resource group inside a Data Landing Zone. Please deploy a [Data Management Landing Zone](https://github.com/Azure/data-management-zone) and [Data Landing Zone](https://github.com/Azure/data-landing-zone) first. |[![Deploy To Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#blade/Microsoft_Azure_CreateUIDef/CustomDeploymentBlade/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-product-batch%2Fmain%2Finfra%2Fmain.json/uiFormDefinitionUri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-product-batch%2Fmain%2Fdocs%2Freference%2Fportal.dataProduct.json) | [Repository](https://github.com/Azure/data-product-batch) |
| Data Product Streaming | Deploys a Data Workload template for Data Streaming Analysis to a resource group inside a Data Landing Zone. Please deploy a [Data Management Landing Zone](https://github.com/Azure/data-management-zone) and [Data Landing Zone](https://github.com/Azure/data-landing-zone) first. |[![Deploy To Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#blade/Microsoft_Azure_CreateUIDef/CustomDeploymentBlade/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-product-streaming%2Fmain%2Finfra%2Fmain.json/uiFormDefinitionUri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-product-streaming%2Fmain%2Fdocs%2Freference%2Fportal.dataProduct.json) | [Repository](https://github.com/Azure/data-product-streaming) |
| Data Product Analytics     | Deploys a Data Workload template for Data Analytics and Data Science to a resource group inside a Data Landing Zone. Please deploy a [Data Management Landing Zone](https://github.com/Azure/data-management-zone) and [Data Landing Zone](https://github.com/Azure/data-landing-zone) first. |[![Deploy To Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#blade/Microsoft_Azure_CreateUIDef/CustomDeploymentBlade/uri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-product-analytics%2Fmain%2Finfra%2Fmain.json/uiFormDefinitionUri/https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2Fdata-product-analytics%2Fmain%2Fdocs%2Freference%2Fportal.dataProduct.json) | [Repository](https://github.com/Azure/data-product-analytics) |

## Deploy Data Product

To deploy the Data Product into your Data Landing Zone, please follow the step-by-step instructions:

1. [Prerequisites](/docs/DataManagementAnalytics-Prerequisites.md)
2. [Create repository](/docs/DataManagementAnalytics-CreateRepository.md)
3. [Setting up Service Principal](/docs/DataManagementAnalytics-ServicePrincipal.md)
4. Template Deployment
    1. [GitHub Action Deployment](/docs/DataManagementAnalytics-GitHubActionsDeployment.md)
    2. [Azure DevOps Deployment](/docs/DataManagementAnalytics-AzureDevOpsDeployment.md)
5. [Known Issues](/docs/DataManagementAnalytics-KnownIssues.md)

## Contributing

Please review the [Contributor's Guide](./CONTRIBUTING.md) for more information on how to contribute to this project via Issue Reports and Pull Requests.
