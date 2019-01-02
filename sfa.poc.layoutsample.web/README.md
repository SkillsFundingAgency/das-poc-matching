### GDS Layout sample

Sample that shows how to set up  basic web iste using GDS styles.processed by back-end functions




Based on the [ApplyService](https://github.com/SkillsFundingAgency/das-apply-service/tree/master/src/SFA.DAS.ApplyService.Web)

you can just copy the /wwwroot folder but remember to empty the application.css file as that has all the custom styles for the apply project.

Other things of note are the `bundleconfig.json` file

https://github.com/SkillsFundingAgency/das-apply-service/blob/master/src/SFA.DAS.ApplyService.Web/Views/Shared/_Layout.cshtml
Again.. you might want to ditch a few of the sections in _Layout, such as the ‘das-user-panel’.
I’d take a look at another view pulled in by `@RenderBody()` as they all need to contain a `<main/>` element.

This is a nice simple example. 
The start page:
https://github.com/SkillsFundingAgency/das-apply-service/blob/master/src/SFA.DAS.ApplyService.Web/Views/Home/Index.cshtml (edited) 


- Added wwwroot assets
- added javascripts
- added stylesheets

added bundleconfig.json

- removed images, css, lib, js and favicon

- cleared application.css

- copied layout - but removed user panel and some stuff about 
@using SFA.DAS.ApplyService.Session
@inject ISessionService SessionService


## Configuration

In nlog.config, need to set the appName to one appropriate for the application.
Make sure the following is in .gitignore:
'''*-nlog.txt'''

May need to add SessionRedisConnectionString - this is in the Apply Web sample.

## TODO

* The signout/login buttons area needs to be styled properly.
* The text on Privacy and other pages needs to be updated for the new application.
* Did not include `inactive_app_offline_private_beta.html` - should this be in?

