---
id: navigation
title: Dyndle Navigation
sidebar_label: Dyndle Navigation
---

## Install

Add a reference to the NuGet package: Dyndle.Modules.Navigation

Note: the Dyndle installer has created a TBB called Sitemap, which is used in a page template called Navigation, which is - in turn - used by the page /system/navigation.json. If you don't have this page, you need to create it manually and publish it.

Also note that the location of this navigation page can be configured in the Web.config (using Navigation.SourceUrl, as explained below). If, for whatever reason, you want to use a different location, make sure the Web.config is updated accordingly.

## Using Navigation Helper Methods

The Navigation module gives you a number of HTML helper methods. With these, you can easily write out various forms of web site navigation from any view. 

Here is an example of how to write out the first two levels of the top navigation:

```html
<ul class="main-nav">
    @foreach (var item in Html.Navigation().Items)
    {
        <li>
            <a href="@item.Url">@item.Title</a>
            @if (item.Items != null && item.Items.Any())
            {
                <ul>
                    @foreach (var childItem in item.Items)
                    {
                        <li>
                            <a href="@childItem.Url">@childItem.Title</a>
                        </li>
                    }

                </ul>
            }
        </li>
    }
</ul>
```
The method Html.Navigation() returns an object of type ISitemapItem. The most important properties of this type, are:

- string Title (the title of the structure group or page in Tridion, without the numeric prefix) - string Id (the TCM URI of the structure group or page in Tridion)
- string Url (the URL of the page or structure group)
- string Type (set to "page" for pages and to "structuregroup" for structure groups)
- bool Visible (indicates whether or not the page or structure group should be visible, see under Publishing logic further down on this page)

The Navigation() method is used to generate 'top navigation' - it starts with the root of the site and includes the structure groups and pages below it. This is typically what you use to build a top navigation / mega dropdown sort of structure.

There are other options as well, which can be used to write out other forms of navigation. For example: you can create bread crumb navigation with Html.NavigationPath(). The full list of options are described below, under Types of navigation.

### Rendering navigation with a separate view
The helper methods are divided into two types, one's that begin **_Render_** and the other that begin with **_Navigation_**. The difference between the two is that in the **_Render_** methods take a _viewName_ as a parameter and render the view directly whereas the **_Navigation_** methods return an **object** that can be used by the user, modified and later on rendered.

An example of how to use a **Render** method:

```html
@{
    Html.RenderNavigationBreadcrumbs("Breadcrumbs");
}
```

This will render a view called 'breadcrumbs' with the navigation path to the current page.

## Types of navigation
The following table describes the functionalities of the navigation module and contains some example implementions to use as a reference.


| Command  |Description | Example |
| :--- | :--- | --- |
| Navigation           | This method is used for rendering navigation items starting from the root of the site | `@Html.Navigation();`                        |
| Navigation(int level)           | You can specify the depth of the navigation (e.g. only include the first 2 levels) | `@Html.Navigation(2);`                        |
| NavigationSideNav           | This method is used for Rendering Sidebar navigation items (siblings of the current page) | `@Html.NavigationSideNav();`                        |
| NavigationBreadcrumbs       | This method is used for Rendering breadcrumbs for the current page.                       | `@Html.NavigationBreadcrumbs();`                    |
| NavigationSitemap           | This is used to generate a sitemap for the entire site.                                   | `@Html.NavigationSitemap();`                        |
| RenderNavigation     | Renders the top navigation using the specified view                          | `@Html.RenderNavigation("TopNavigation");`         |
| RenderNavigationSideNav     | Renders the result of NavigationSideNav using the specified view                          | `@Html.RenderNavigationSideNav("Sidebar");`         | 
| RenderNavigationBreadcrumbs | Renders the result of NavigationBreadcrumbs using the specified view                      | `@Html.RenderNavigationBreadcrumbs("Breadcrumbs");` |
| RenderNavigationSitemap     | Renders the result of NavigationSitemap using the specified view                          | `@Html.RenderNavigationSitemap("Sitemap");`         |

## Publishing logic

All navigation methods read data from a single page (normally located in /system/navigation.json). This page is generated by the Navigation page template, based on the following business rules:

- Pages with a title starting with an underscore ('_') are excluded from the navigation
- Structure groups are always included in the navigation, but:
    - If their title starts with a one or more numbers followed by a space, the attribute 'Visible' is set to true
    - If not, the attribute 'Visible' is set to false

The reason for including all structure groups (with the Visible attribute), instead of omitting them altogether, is that some forms of navigation (like bread crumbs) may be required even if the page itself never shows up in the navigation.
You can use 

If you want to modify the logic behind the Navigation page, we recommend that you copy the source of the Sitemap TBB in the Dyndle template repository, and create your own version. You can find it here: https://github.com/dyndle/dyndle-templates/blob/master/src/Dyndle.Templates/Sitemap.cs.


## Google sitemap.xml

The navigation module can generate a fully functional sitemap.xml (aka "Google Sitemap"). All you need to do is enable this feature by adding an appSetting key called 'Navigation.SitemapEnabled'.
Of course, you can also write out the sitemap.xml yourself, so you can customize the functionality. All you need to do is create a controller and inject the INavigationProvider into it. From this provider, you can retrieve the full sitemap using the GetFullSitemap() method.

## Configuration

The following table describes the configuration values in the appsettings section of the web config that are used in the Dyndle Image Enhancement module.

| Config key                          | Default Value             | Description                                                                                                                         |
| :---------------------------------- | :------------------------ | :---------------------------------------------------------------------------------------------------------------------------------- |
| Navigation.SourceUrl                | `/system/navigation.json` | Setting that determines the path to the page containing the navigation source. Defaults to  /system/navigation.json                                                                      |
| Navigation.SubNavDefaultsToMainNav  | `false`                   | If true, the subnavigation is read from the root for pages whose level falls below the start level                                  |
| Navigation.IncludeAllPagesInSitemap | `true`                    | Setting that determines whether to include all pages in sitemap. If set to false empty structuregroups are removed from the sitemap |
| Navigation.SitemapEnabled | `true`                    | If true, a Google sitemap is automatically shown when you request /sitemap.xml in the browser. It also works for /\<country>/sitemap.xml, and /\<country>/\<language>/sitemap.xml. |