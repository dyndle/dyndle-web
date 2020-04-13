---
id: xpm
title: Working with Experience Manager
sidebar_label: Experience Manager
---

Dyndle offers out of the box support for Experience Manager, Tridion's in-context editing feature. 

You need to take the following steps:

- Enable the AmbientData framework
- Configure Experience Manager
- Change your page views
- Change your entity (component) views


## Before you start
Make sure you have a session-enabled content service and a preview service set up. The preview service must be configured to allow 'taf:session:preview:preview_session' as one of the GloballyAcceptedClaims.

It is not necessary to configure anything on the Content Manager side, and you don't have to add any special template building block to your templates. 

## Enabling AmbientData
The Ambient Data Framework (ADF) is Tridion’s mechanism to pass information about the context to the microservices. Without this, the preview token is not passed to the service – the content is retrieved from the broker and not the preview DB during session preview.

Open the Web.config and add the following module to the modules element inside the system.webServer element:

```xml
<add type="Tridion.ContentDelivery.AmbientData.HttpModule" name="Tridion.ContentDelivery.AmbientData.HttpModule" preCondition="managedHandler" />
```

If you don't have a system.webServer or modules element yet, just create them.

Next, you need to make sure there is a file called cd_ambient_conf.xml in the bin\config folder within your website. You can find an example file in the Tridion installation (Content Delivery\roles\api\rest\dotnet\config\cd_ambient_conf.xml). Most importantly, make sure you have configured 'taf:session:preview:preview_session' as one of the ForwardedClaims, like this:

```xml
<ForwardedClaims CookieName="TAFContext">
    <Claim Uri="taf:session:preview:preview_session"/>
</ForwardedClaims>
```

### Post-build action
When you rebuild a web application from VS, the bin folder is wiped clean every time. This will delete the config folder with the cd_ambient_conf.xml in it.

To work around this, you can create a config folder somewhere else (e.g. in a folder called 'config' in the root of your repository), and use a post-build action to copy it to the correct location whenever you build your project, like this:

```shell
xcopy "$(SolutionDir)..\config" "$(TargetDir)config" /Y /E /I
``` 


## Configuring Experience Manager
Open the Web.config and add the following appSettings:

| Config key                          | Value            | Description |
| :---------------------------------- | :----------------| :------------------------------------------------------------ |
| DD4T.IsPreview                      | true  | Tells DD4T that the site should support session preview       |
| DD4T.ContentManagerUrl         |       | URL of the Tridion Content Manager in the current environment |
| Dyndle.StagingSite                  | true  | Tells Dyndle that this is a staging site                      |


## Change your page views
Now it's time to make some changes to the page views. Find the page view you want to enable XPM for, and add this at the bottom:

```
@Model.InsertXpmPageMarkup();
```

Make sure the model of your Razor view is a DD4T ViewModel which represents a page in Tridion. If you want to add this line to your layout, simply make sure the layout uses a valid ViewModel. One way to do this is to configure the layout to use the Dyndle WebPage, which is the super class of your page models:

```
@model Dyndle.Modules.Core.Models.WebPage
```

Also, you may need to add the following using statement to the top of your view:

```
@using DD4T.Mvc.ViewModels.XPM
```

### Clean up the session preview cookie
If you use session preview, the preview token cookie is set by Tridion. It will stay active until the browser is restarted. Dyndle makes sure that caching of pages and component presentations is disabled while this cookie is present.
This means that the caching will not work anymore, even if you leave XPM and go back to the regular staging site.
As a work-around, you can put this in your page views, after the bootstrap:

```
@Html.CleanupPreviewCookie()
```

This generates a bit of JavaScript which removes the preview token cookie at the end of each request. Since it is set again by Experience Manager when it needs to run a preview, this is safe to do.


## Change your entity views
Your entity views must be changed for two reasons:

1. To tell XPM where the component presentations are
2. To tell XPM where the fields are

### Component presentations
For each component presentation, add the following code to your views:

```
<div>
    @Model.StartXpmEditingZone()
    ... The rest of the view
</div>
```
Again, you may need to add the a using statement to include the namespace DD4T.Mvc.ViewModels.XPM.

Note that each component presentation must always be in its own HTML element. This is a prerequisite if you want to work with XPM, it is not specific to Dyndle or DD4T! 
The element doesn't have to be a div by the way, it can be anything you like (span, article, section, etc).

### Fields
For each field you are writing out, add something like this:

```
<span>
  @Model.XpmEditableField(m => m.Company)
</span>
```

Fields (like component presentations) must be in their own HTML element, which can be anything (not just a span).






