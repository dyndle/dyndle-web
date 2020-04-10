---
id: navigation
title: Dyndle Navigation
sidebar_label: Dyndle Navigation
---

## Install

NuGet package: Dyndle.Modules.Navigation

## Using Navigation Helper Methods

The following table describes the functionalities of the navigation module and contains some example implementions to use as a reference.

The helper methods are divided into two types, one's that begin **_Render_** and the other that begin with **_Navigation_**. The difference between the two is that in the **_Render_** methods take a _viewName_ as a parameter and render the view directly whereas the **_Navigation_** methods return an **object** that can be used by the user, modified and later on rendered.

| Command                     | Description                                                                               | Example                                             |
| :-------------------------- | :---------------------------------------------------------------------------------------- | --------------------------------------------------- |
| NavigationSideNav           | This method is used for Rendering Sidebar navigation items (siblings of the current page) | `@Html.NavigationSideNav();`                        |
| NavigationBreadcrumbs       | This method is used for Rendering breadcrumbs for the current page.                       | `@Html.NavigationBreadcrumbs();`                    |
| NavigationSitemap           | This is used to generate a sitemap for the entire site.                                   | `@Html.NavigationSitemap();`                        |
| RenderNavigationSideNav     | Renders the result of NavigationSideNav using the specified view                          | `@Html.RenderNavigationSideNav("Sidebar");`         |
| RenderNavigationBreadcrumbs | Renders the result of NavigationBreadcrumbs using the specified view                      | `@Html.RenderNavigationBreadcrumbs("Breadcrumbs");` |
| RenderNavigationSitemap     | Renders the result of NavigationSitemap using the specified view                          | `@Html.RenderNavigationSitemap("Sitemap");`         |
