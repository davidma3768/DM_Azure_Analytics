# Dedicated SQL Pool Performance Tuning Framework

## Outline of Activities
In the ideal world, everything is covered by the guidline, everyone follows the best practice, so we barely see the performance issue ... in the ideal world. In the real world, you may have to work with customer on a project that you don't have direct access to the customer environment, and there could be legacy configuration that you are not aware of. When you are seeing an unfamiliar SQL pool for the first time, it is important that you do not assume anything about the customer’s environment. A performance troubleshooting framework and a set of tool can build a solid foundation ...

## Checkpoints
Serveral checkpoint categories base on the activity outline: System Profile, Table Design, Metadata Check, Feature usage check
| Category   | Items | Scripts | Description |
|:-----------|:---------------------|:----------------|----------------------------------|
| System Profile| Performance Setting Enabled | [Database Settings Enabled.sql](https://github.com/microsoft/AzureSynapseScriptsAndAccelerators/blob/main/Scripts/Dedicated%20SQL%20pool/System%20Profile/Database%20Settings%20Enabled.sql) | To check if the following performance related settings are enabled <br> -- ***Read_committed_Snapshot*** <br> -- ***Auto_Create_Stats*** <br> -- ***Query Store*** <br> -- ***Result_Set_Caching*** |
| System Profile | SLO (DWUs) | [Get Database SLO.sql](https://github.com/microsoft/AzureSynapseScriptsAndAccelerators/blob/main/Scripts/Dedicated%20SQL%20pool/System%20Profile/Get%20Database%20SLO.sql) | runs against the MASTER DATABASE and returns the SLO (DWUs) for each DW attached to the logical server |
| System Profile | Items | Get Database Object Count.sql | Description |
