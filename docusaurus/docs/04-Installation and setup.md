---
id: installation
title: Installation and setup
sidebar_label: Installation and setup
---

In order to use Dyndle, following steps must be carried out:

- Install NuGet package(s)
- Make sure Dyndle features are loaded in the application
- Add configuration (see [next chapter](configuration))
- Make sure there all neccessary items are present in Tridion (see [Dyndle CLI](cli)).

## Dyndle Core

### Install

NuGet package: Dyndle.Modules.Core

### Load

Core module contains a large amount of crucial functionality that is required for Dyndle to operate properly.

_To be rewritten once with explanation of StartDyndle approach_

## Dyndle Management

### Install

NuGet package: Dyndle.Modules.Management

### Load

Only controllers from this module need to be registered in the DI container.

## Dyndle Navigation

### Install

NuGet package: Dyndle.Modules.Navigation

### Using Navigation Helper Methods

The following table describes the functionalities of the navigation module and contains some example implementions to use as a reference. 

The helper methods are divided into two types, one's that begin ***Render*** and the other that begin with ***Navigation***. The difference between the two is that in the ***Render*** methods take a *viewName* as a parameter and render the view directly whereas the ***Navigation*** methods return an **object** that can be used by the user, modified and later on rendered.

| Command                  | Description                                                  | Example                                      |
| :----------------------- | :----------------------------------------------------------- | -------------------------------------------- |
| NavigationChildren       | This method is used for Rendering Sidebar navigation items (siblings of the current page) | `@Html.NavigationChildren();`                |
| NavigationPath           | This method is used for Rendering breadcrumbs for the current page. | `@Html.NavigationPath();`                    |
| NavigationSitemap        | This is used to generate a sitemap for the entire site.      | `@Html.NavigationSitemap();`                 |
| RenderNavigationChildren | Renders the result of NavigationChildren into a view         | `@Html.RenderNavigationChildren("Sidebar");` |
| RenderNavigationPath     | Renders the result of NavigationPath into a view             | `@Html.RenerNavigationPath("Breadcrumbs");`  |
| RenderNavigationSitemap  | Renders the result of NavigationSitemap into a view          | `@Html.RenderNavigationSitemap("Sitemap");`  |

## Dyndle Search

### Install

NuGet package: Dyndle.Modules.Search

### Load

...

## Dyndle Image Enhancement

### Install

1. Install the Dyndle.ImageEnhancement nuget package, the nuget package will automatically add some keys to your appsettings in the web.config. Refer to the configuration section for details on the configuration options.

2. Add these two keys to the namespaces section in your views web.config for convenience.

   `<add namespace="Dyndle.Modules.ImageEnhancement.Html"/>`

   `<add namespace="Dyndle.Modules.ImageEnhancement.Models"/>`

3. Make sure you have this entry in the system.webServer section of your Web.config

   `<modules runAllManagedModulesForAllRequests="true" />`

   

### Using Image Enhancement

The following table describes the functionalities of the image enhancement module and contains some example implementions to use as a reference.

| Command                 | Description                                                  | Example                                                      |
| ----------------------- | ------------------------------------------------------------ | ------------------------------------------------------------ |
| SetWidth                | Scale the image to the specified width (height will be scaled proportionally). | `<img src="@Model.Multimedia.Url.SetWidth(200)" />`          |
| SetHeight               | Scale the image to the specified height (width will be scaled proportionally). | `<img src="@Model.Multimedia.Url.SetHeight(200)" />`         |
| SetWidthAndHeight       | Scale the image to the specified width and height. If the aspect ratio differs from the original, the image will be padded with a background color that is specified in the Web.config, like this: `<appSettings>   <add key="ImageEnhancement.BackgroundColor" value="#f00" /> </appSettings>` | `<img src="@Model.Multimedia.Url.SetWidthAndHeight(300, 200)" />` |
| SetCropCenter           | Crop around the specified center. Center is specified in pixels (x and y).Must always be combined with SetWidthAndHeight. | `<img src="@Model.Multimedia.Url.SetWidthAndHeight(200, 200).SetCropCenter(110,100)" />` |
| SetCropCenterPercentage | Crop around the specified center. Center is specified in percentage (x and y).Must always be combined with SetWidthAndHeight. | `<img src="@Model.Multimedia.Url.SetWidthAndHeight(200, 200).SetCropCenterPercentage(75,50)" />` |
| SetCropStyle            | There are two crop styles: Greedy (default): take the largest possible area around the center point and scale that to the specified size (image may be scaled down)NonGreedy: take exactly the specified size around the center point (image maintains the same scale) | `<img src="@Model.Image.Multimedia.Url.SetWidthAndHeight(300, 200).SetCropCenterPercentage(75,50).SetCropStyle(CropStyle.NonGreedy)" />` |