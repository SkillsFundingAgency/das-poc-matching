# Azure Search POC #

POC to investigate Azure Search.

Uses the same table configuration as the [Configuration Sample](../sfa.poc.matching.configuration/README.md).

The following needs to be in the config json (with appropriate connection strings):
```
  "EnvironmentName": "LOCAL",
  "ServiceName": "SFA.DAS.Matching",
  "Version": "1.0"
```

The following needs to be in the configuration table:

```
{
  "SqlConnectionString": "Data Source=(localdb)\\ProjectsV13;Initial Catalog=SFA.POC.Matching.Database;Integrated Security=True;MultipleActiveResultSets=True;",
  "AzureSearchConfiguration":
  {
    "SearchServiceName": "Put your search service name here",
    "SearchServiceAdminApiKey": "Put your primary or secondary API key here",
    "SearchServiceQueryApiKey": "Put your query API key here"
  }
},

```

The search uri is `https://matchingpoc.search.windows.net`

## Things to look at

[https://docs.microsoft.com/en-us/azure/search/search-synonyms-tutorial-sdk](Synonyms)

