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
    "WtRealm":"<replace with the application identifier>",
    "MetaDataEndpoint": "<replace with meta data endpoint url>"
  }
}
```

In the PoC ``WtRealm` was set to be the same as the https endpoint of the application, found in sfa.poc.matching.staff.idams\Properties\launchSettings.json 

# Notes from Recruit POC: 

## Provider & Staff

Relying parties are managed by Core DevOps team. Change requests are needed for changes to both the PreProd and the Prod environments. They'll need the following info:
* An application Identifier (can be any string)
* Url of the application (**must** use https)


Update the following `appSettings.json` values:

```json
{
  "Authentication": {
    "WtRealm": "<replace with the application identifier>",
    "MetaDataEndpoint": "<replace with meta data endpoint url>"
  }
}
```


##Questions

this: app.UseAuthentication(); // TODO: WP - Add the authentication middleware into the pipeline

Do we need this - if so should be coming from config
Program.cs - .UseUrls("https://localhost:5025");
