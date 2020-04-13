---
id: image-enhancement
title: Dyndle Image Enhancement
sidebar_label: Dyndle Image Enhancement
---

## Install

1. Install the Dyndle.ImageEnhancement nuget package, the nuget package will automatically add some keys to your appsettings in the web.config. Refer to the configuration section for details on the configuration options.

2. Add these two keys to the namespaces section in your views web.config for convenience.

   `<add namespace="Dyndle.Modules.ImageEnhancement.Html"/>`

   `<add namespace="Dyndle.Modules.ImageEnhancement.Models"/>`

3. Make sure you have this entry in the system.webServer section of your Web.config

   `<modules runAllManagedModulesForAllRequests="true" />`

## Configuration
You can configure the image enhancement feature with the following appSettings in the Web.config.

| Config key                       | Example Value     | Description                                                  |
| -------------------------------- | ----------------- | ------------------------------------------------------------ |
| ImageEnhancement.Localpath       | "/EnhancedImages" | Where the enhanced versions of the images will be stored (defaults to /EnhancedImages)   |
| ImageEnhancement.Backgroundcolor | "#fff"            | The color that is used when you crop an image and provide a width and height that's higher than the cropped area. |
| ImageEnhancement.Cacheseconds    | "300"              | How long the enhanced image will be retrieved from the cache before creating a new enhanced version. (defaults to 300) |

Note that the caching of enhanced images is independent from the normal DD4T / Dyndle caching. It is NOT possible to automatically delete enhanced images when the parent image is unpublished or republished (decaching). It is generally advisable to keep the value low. 

## Using Image Enhancement

The following table describes the functionalities of the image enhancement module and contains some example implementions to use as a reference.

| Command                 | Description                                                                                                                                                                                                                                                                                   | Example                                                                                                                                  |
| ----------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| SetWidth                | Scale the image to the specified width (height will be scaled proportionally).                                                                                                                                                                                                                | `<img src="@Model.Multimedia.Url.SetWidth(200)" />`                                                                                      |
| SetHeight               | Scale the image to the specified height (width will be scaled proportionally).                                                                                                                                                                                                                | `<img src="@Model.Multimedia.Url.SetHeight(200)" />`                                                                                     |
| SetWidthAndHeight       | Scale the image to the specified width and height. If the aspect ratio differs from the original, the image will be padded with a background color that is specified in the Web.config, like this: `<appSettings> <add key="ImageEnhancement.BackgroundColor" value="#f00" /> </appSettings>` | `<img src="@Model.Multimedia.Url.SetWidthAndHeight(300, 200)" />`                                                                        |
| SetCropCenter           | Crop around the specified center. Center is specified in pixels (x and y).Must always be combined with SetWidthAndHeight.                                                                                                                                                                     | `<img src="@Model.Multimedia.Url.SetWidthAndHeight(200, 200).SetCropCenter(110,100)" />`                                                 |
| SetCropCenterPercentage | Crop around the specified center. Center is specified in percentage (x and y).Must always be combined with SetWidthAndHeight.                                                                                                                                                                 | `<img src="@Model.Multimedia.Url.SetWidthAndHeight(200, 200).SetCropCenterPercentage(75,50)" />`                                         |
| SetCropStyle            | There are two crop styles: Greedy (default): take the largest possible area around the center point and scale that to the specified size (image may be scaled down)NonGreedy: take exactly the specified size around the center point (image maintains the same scale)                        | `<img src="@Model.Image.Multimedia.Url.SetWidthAndHeight(300, 200).SetCropCenterPercentage(75,50).SetCropStyle(CropStyle.NonGreedy)" />` |
