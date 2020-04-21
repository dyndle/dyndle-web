---
id: modelling-includes
title: Modelling includes
sidebar_label: Modelling includes

---

Includes are pages that are designated to be reusable inserts across a web application. You can configure an Url to be used as an includes page. Then you can include the full page content via `Html.RenderIncludes` or render a specific region of the include page using `Html.RenderIncludesRegion`. This allows you to control site-wide regions, such as cookie opt-in, header and footer, in a centralized place. Include pages are also cached once, so they bring performance benefits.

To setup a page as an includes page, first you have to configure the path to the page using the `Dyndle.IncludesUrl` configuration key. Let's say you have a `includes` structure group where you store your include pages. The value of the configuration key would be: `/includes/filename` where the filename matches the filename in Tridion for the page you wish to use. The extension of the file is not necessary to add. 

Now you need to create a view that uses the `IncludePage` model provided by the Dyndle core module. You can then extract the entities from the page and cast them to a model of the component that you put on the include page. Below is an example of how to implement this.

```c#
<!-- Use the Dyndle IncludePage for the model -->
@model Dyndle.Modules.Core.Models.System.IncludePage
@{
    <!-- Check if the model contains any entities -->
    if (Model.Entities.Any())
    {
        <!-- Loop through the entities and render the HTML -->
        foreach (var entity in Model.Entities)
        {
            <!-- Cast the entities to the desired entity model -->
            var yourEntity = entity as YourEntityModel;

            <h1>@yourEntity.Title</h1>
            <p>@yourEntity.Content</p>
        }
    }
}
```

With the configuration and the view in place you can now use the HTML helper functions to render the includes in any part of the web application! There are a couple ways how you can use this:

`@Html.RenderIncludes("viewname")` Where the viewname is the name of the view that uses the `IncludePage` model. This will use the configured Url to find the page in Tridion.

`@Html.RenderIncludes("includename","viewname")` Where you can provide the name of the page in Tridion as an argument. This requires an additional configuration key: `Dyndle.IncludesUrlMask` that contains the path to the folder where one or more includepages are stored. With the previous example of an `includes` folder in Tridion that path would look like: `/includes/{0}` where the bracketed zero will be replaced with the filename provided as an argument to the html helper. This will then result in the same `/includes/filename` Url, but can be easily pointed to a different page without changing the configuration.

`@Html.RenderIncludesRegion("region", "viewname")` This function will only render the components that are coupled to the region provided as an argument.

`@Html.RenderIncludesRegion("includename","region", "viewname")` Will also only render the components from the supplied region but will use the `Dyndle.IncludesUrlMask` path to create the Url.

`@Html.RenderIncludesByUrl("includesurl", "region","viewname")` With which you can directly supply the includes Url of the page, the region and the viewname to use without the use of any configuration values.



 