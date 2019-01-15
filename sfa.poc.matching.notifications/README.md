### Notifications POC ###

POC to see how to configure and use the Notifications API for sending emails


## Notes from Deven:

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

## gov Notify


Add the https://api.bintray.com/nuget/gov-uk-notify/nuget package source to your project.
Install package Notify



Notes from Engineering - tidy this up and extend the POC to use the new portal.
Ewan Noble [10:24 AM]
@Mike Wild @Hardik Desai morning chaps, you'll probably need to look at implementing this into your app https://docs.notifications.service.gov.uk/net.html#net-client-documentation , the notifications API you're trying to use is backed by a DAS account in Gov Notify which we won't be using for TLevels

Mike Wild [10:30 AM]
@Hardik Desai I'll add this on as a "new" notify POC - doesn't look like it would be too different from what I've already developed except it will be a different package,

Ewan Noble [10:30 AM]
can i have your email addresses to invite you

Hardik Desai [10:31 AM]
hardik.desai@digital.education.gov.uk

Mike Wild [10:32 AM]
mike.wild@digital.education.gov.uk

Hardik Desai [10:32 AM]
@ewan.noble_esfa look like this client uses API Key as authentication am I correct?

Adam Underwood [10:33 AM]
adam.underwood@digital.education.gov.uk

Mike Wild [10:34 AM]
@Hardik Desai should we invite any of our designers?

Hardik Desai [10:35 AM]
not really, as long as they approve the email template, they dont need to see technical implementation
we will have to invite the UI guy once he joins us

Mike Wild [10:35 AM]
I'm in, thanks @ewan.noble_esfa

Hardik Desai [10:37 AM]
I have also signed up
@ewan.noble_esfa is there a preprod or dev version of this?
or we just integrate with live in all environments?

Ewan Noble [10:40 AM]
that depends on how you want to manage the templates

Hardik Desai [10:42 AM]
if no restriction to use live service then that reduces complexity
Your service is in trial mode
you can only send messages to yourself
you can add people to your team, then you can send messages to them too
you can only send 50 messages per day
To remove these restrictions request to go live.
this is at the bottom of settings page

Ewan Noble [10:44 AM]
is that not fine for now?

Hardik Desai [10:45 AM]
no issue just I saw that and it kind of answers my previous questions
and there is plenty of documentation on using this service in test environments, so that also answers all the questions
sorry one quick questions, looking at all the documentation and api key settings, it seems that having this account makes us administrators of this service, do we need to secure the Api Key? (anti Personas)

Ewan Noble [10:53 AM]
ill create the live keys and store them safely
your access will probably be revisited before this is productionised

Hardik Desai [10:54 AM]
cool, I can see there is Manage API integration permission in Team members page

Mike Wild [10:54 AM]
Don't check it into git :slightly_smiling_face: We will need to keep the API key in our configuration table - @ewan.noble_esfa will you add a setting for our notification settings? Feel free to remove/rename anything we won't need like the token

Hardik Desai [10:56 AM]
this is the only service I have seen since I have joined that seems to be not chaotic :wink:

Ewan Noble [10:57 AM]
don't speak too soon :slightly_smiling_face:

Hardik Desai [10:57 AM]
:slightly_smiling_face: