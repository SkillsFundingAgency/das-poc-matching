### Notifications POC ###

POC to see how to configure and use the Notifications API for sending emails


### Notes from Deven:

1. Define Templates on Notify portal https://www.notifications.service.gov.uk/services/ffe171ac-7dde-4299-9bb1-b9cee62af4c7/templates
2. Configure template on das-employer-config https://github.com/SkillsFundingAgency/das-employer-config/tree/master/das-notifications
3. In code, refer notifications api nuget package https://www.nuget.org/packages/SFA.DAS.Notifications.Api.Client (edited) 
notifications.service.gov.uk
Sign in – GOV.UK Notify

you will need a login for notify portal (step 1) and you will need a JWT token for (step 3)... DevOps should be able to do this for you.


you will need this in config...

  "NotificationsApiClientConfiguration":{
      "ApiBaseUrl": "https://at-notifications.apprenticeships.sfa.bis.gov.uk/",
      "ClientToken": ""
    },

See also https://github.com/SkillsFundingAgency/das-notifications
