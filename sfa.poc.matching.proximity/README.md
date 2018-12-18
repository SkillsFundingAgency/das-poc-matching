# Proximity POC #

POC to investigate options for postcode lookup, proximity searching and distance calculation.

Uses the same table configuration as the [Configuration Sample](../sfa.poc.matching.configuration/README.md).

The following needs to be in the config json (with appropriate connection strings):
```
  "EnvironmentName": "LOCAL",
  "ServiceName": "SFA.DAS.Matching",
  "Version": "1.0"
```

```
{
  "SqlConnectionString": "Data Source=(localdb)\\ProjectsV13;Initial Catalog=SFA.POC.Matching.Database;Integrated Security=True;MultipleActiveResultSets=True;",
  "CosmosConnectionString": "TODO",
  "PostCodeRetrieverBaseUrl": "https://postcodes.io/postcodes",
  "Authentication":
  {
    "WtRealm":"unknown",
    "MetaDataEndpoint": "unknown"
  }
}
```

#Console Application

The console application in the POC asks for either a postcode or "/r" followed by a number. The first will look up and save a single postcode; the second will request a sample of random postcodes (up to the number after "/r") and persist these to the database.

##Known issues

Not all postcodes are returned with a latitude and longitude. We might be able to look these up via another service such as postcodes anywhere. 
As an initial solution, the POC attempts to get a location from the Out Code (the first part of the post code)

