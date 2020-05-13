---
id: model-binders-and-controllers
title: Model binders and controllers
sidebar_label: Model binders and controllers
---

Dyndle comes with a set of controllers to handle the retrieval and display of content from Tridion. There is a PageController for pages, a RegionController for regions (groups of component presentations), an EntityController for component presentations (called 'entities' in Dyndle), and a BinaryController for binaries.

If you look at the source code of these controllers, you will see they are very small. This is possible because the 'hard work' is done behind the scenes, in so-called Model Binders. See https://www.c-sharpcorner.com/article/introduction-to-asp-net-mvc-model-binding/ if you don't know what these are.

If you use the Dyndle Quickstart module, all routing for these controllers is configured automatically. If you don't, you need to create the routes yourself. Open the RouteConfig class (it's usually in the App_Start folder) and add the following routes:

```c#
// route for binaries, based on a configurable pattern (see Dyndle.BinaryUrlPattern in Web.config)
routes.MapRoute(
    "Core_Binaries",
    "{*url}",
    new { controller = "Binary", action = "Binary" },
    new { url = String.IsNullOrEmpty(DyndleConfig.BinaryUrlPattern) ? DEFAULT_BINARY_URL_PATTERN : DyndleConfig.BinaryUrlPattern });

// Tridion Page Route
routes.MapRoute(
    "Core_PageModel",
    "{*page}",
    new { controller = PageControllerName, action = PageActionName },
    new { page = new RedirectInvalidPageConstraint() }
).DataTokens.Add("area", "Core");
```



