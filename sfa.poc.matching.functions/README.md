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

## Known issues

* There is a bug in the latest version of functions where Visual Studio will not automatically create an `extensions.json` file pointing at the startup class. To work around this the file has been added to the project and set to be copied to the build folder on build.
>>* This might be fixed in a new release of the functions NuGet package. 

* `extensions.json` is not copied to the bin folder when published to Azure. A `Directory.Build.targets` file has been added to work around this, as suggested [here](https://github.com/Azure/azure-functions-host/issues/3386)