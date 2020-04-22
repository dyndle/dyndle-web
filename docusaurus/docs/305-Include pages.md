---
id: include-pages
title: Including content from other pages
sidebar_label: Including content from other pages
---

Includes are pages that are designated to be reusable inserts across a web application. You can configure an Url to be used as an includes page. Then you can include the full page content via `Html.RenderIncludes` or render a specific region of the include page using `Html.RenderIncludesRegion`. This allows you to control site-wide regions, such as cookie opt-in, header and footer, in a centralized place. Include pages are also cached once, so they bring performance benefits.


## Render full include page

To setup a page as an include page, first you have to configure the path to the page using the  configuration key. The value of this key should point to the url of the page you want to include. This path consists of the filename of the page in tridion and any structure groups other than the root. So for instance a page with a filename of `footer.html` that exists in the root structure group would have the following entry in the appsettings section of the web.config.

`<add key="Dyndle.IncludesUrl" value="footer.html" />` (the file extension is not required)

If the page would be part of a `includes` structure group the entry would look like this:

`<add key="Dyndle.IncludesUrl" value="/includes/footer.html" />`

With the config entry in place the next step is to create a view that uses the `IncludePage` model provided by the Dyndle core module. You can then extract the entities from the page and render them using their own views. Below is an example of how to implement this.

```c#
<!-- Use the Dyndle IncludePage for the model -->
@model Dyndle.Modules.Core.Models.System.IncludePage
@{
    <!-- Loop through the entities and render the HTML -->
    foreach (var entity in Model.Entities)
    {
    	@Html.RenderEntity(entity)
    }
}
```

Now you can use the HTML helper functions to render the includes in any part of the web application! 

`@Html.RenderIncludes("viewname")`

Where the viewname is the name of the view that uses the `IncludePage` model. This function will use the configured `Dyndle.IncludesUrl`to find the page in Tridion.


## Rendering multiple include pages

Dyndle includes also provide a way to easily use multiple pages as includes. For this you can use the web.config setting: `Dyndle.IncludesUrlMask`. Instead of having a path to a single page you can provide a path to a structure group that contains multiple pages. Let's say you have a `includes` structure group where you store your include pages. The entry in the config would look like this:

`<add key="Dyndle.IncludesUrlMask" value="/includes/{0}" />`

The bracketed zero will be replaced with the filename provided as an argument to the html helper like so:

`@Html.RenderIncludes("footer","viewname")`

This will then result in the same `/includes/footer` Url, but can be easily pointed to a different page without changing the configuration.


## Rendering include pages by region

Include pages can also make use of regions to only render a specific part of the page. For instance, if you have a include page containing a header and a footer region and you only want to render the footer region on a specific page, you can provide that region as an argument in the htmlhelper function and it will only render the components from that region like this:

`@Html.RenderIncludesRegion("region", "viewname")`

In this case you can use the `IRegionModel` from the Dyndle core module models to render the regions.

```c#
<!-- Use the Dyndle IRegionModel for the model -->
@model Dyndle.Modules.Core.Models.IRegionModel
@{
    <!-- Loop through the entities and render the HTML -->
    foreach (var entity in Model.Entities)
    {
    	@Html.RenderEntity(entity)
    }
}
```

Similarly to rendering with a basic include page you can also provide a `Dyndle.IncludesUrlMask` setting in the web.config and use the command with an additional parameter for the filename of the page like so:

`@Html.RenderIncludesRegion("filename","region", "viewname")` 



 