# Staff IDAMS POC #

POC to see how to implement staff IDAMS.

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
  "Authentication":
  {
    "WtRealm":"unknown",
    "MetaDataEndpoint": "unknown"
  }
}
```
