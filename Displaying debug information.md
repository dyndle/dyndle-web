# Displaying debug information

Dyndle makes it possible to view various forms of information directly on the pages. This is intended to help the developers and should NOT be used on a live website.

To enable it, you need to do the following:

- Add a reference to the NuGet package Dyndle.Modules.Management
- Add the following key to the appSettings in your Web.config:

```xml
    <add key="Dyndle.StagingSite" value="true" />
```

- Add the following line to the _Layout.cshtml or another view which is used on all your pages:

```
@Html.ShowDebugInfo()
```

Preferably put this at the bottom of the HTML.

You will now see a number of small, blue buttons appear in the top right corner of the page. They give access to various types of debug information.

TODO: explain what they do.