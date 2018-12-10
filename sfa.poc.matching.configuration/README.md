# Configuration sample #

POC to show how configuration should be loaded from employer-config in a core web app.

This requres the foillowing nuget packages:

```
WindowsAzure.Storage  

```

 * Note: As of 9.4.0, the Table service is not supported by this library. The advice is to  remain on the pre-9.4 WindowsAzure.Storage NuGet package.


Install the Azutre Storage emulator - there's a link on the docs page https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator

Install the Azure storage explorer from https://azure.microsoft.com/en-us/features/storage-explorer/


Configuration needs to be added to [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config)
**(Details to be added)**

Need the following in the config json:
```
  "EnvironmentName": "LOCAL",
  "ServiceName": "SFA.DAS.Matching",
  "Version": "1.0"
```

In the storage explorer 
* Local & Attached .. Storage Accounts .. Emulator - Default Ports (Key) .. Tables

Add a table called `Configuration` and in the table add an entity with

* PartitionKey: LOCAL
* RowKey: SFA.POC.Matching_1.0
* Data as below

```
{
  "SqlConnectionstring": "Data Source=(localdb)\\ProjectsV13;Initial Catalog=SFA.POC.Matching.Database;Integrated Security=True;MultipleActiveResultSets=True;",
    "Authentication":
    {
      "WtRealm": "myrealm",
      "MetaDataEndpoint": "/endpoint"
    }
}
```

The PartitionKey and RowKey should correspond with the values in the local appsettings.json
*  "EnvironmentName": "LOCAL",
*  "ServiceName": "SFA.POC.Matching",
*  "Version": "1.0"

