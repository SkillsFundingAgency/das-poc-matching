# Functions POC #

POC to investigate how to configure and use functions.

## Local Settings ##

The localsettings.json file is gitignored, so you will need to add one and populate it with the following values:

```
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "BlobStorageConnectionString": "UseDevelopmentStorage=true;",
        "CronSchedule": "0 */5 * * * *",
        "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
        "EnvironmentName": "LOCAL",
        "ServiceName": "SFA.POC.Matching",
        "Version": "1.0"

    }
}
```

