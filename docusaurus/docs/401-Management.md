---
id: management
title: Dyndle Management
sidebar_label: Dyndle Management
---

The Management module (Dyndle.Modules.Management) gives you - as a developer - a lot of useful features:

- A cache browser
- Information about the page and the content on it
- Performance analysis

> ### Cache browser
> The cache browser gives insight into the DD4T object cache. You can see exactly what is cached while the application is running. This enables developers and administrators to troubleshoot and monitor the website by investigating what is stored in the cache and what are the dependencies for cache invalidation. You can manually remove individual items from the cache or clear the entire cache completely.

To use this module, you need to do the following:

- Add a NuGet reference to Dyndle.Modules.Management
- Add the following line to the top of the layout view:

```html
@using Dyndle.Modules.Management.Html
```

- And add this to the bottom of the layout view:

```html
@Html.ShowDebugInfo()
```

You will now see a number of buttons appear in the top right corner (next to the Experience Manager button, if you have that enabled). They give access to the various features.






