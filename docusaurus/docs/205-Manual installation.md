---
id: manual-installation
title: Manual web app installation
sidebar_label: Web application - manual installation
---

We do recommend that you first give our [Quickstart](quickstart-installation.html) a go. This is the easiest way to get your application up and running.

The page you are reading now explains how to install Dyndle if you don't want to use the Quickstart!

And of course, you must make sure the [CM-side installation](installation-cm.html) is done before you can start on the web application.

## Overview of steps

In order to build a web application using Dyndle, following steps must be carried out:

- Create an ASP.NET MVC 5 web application
- Install NuGet package(s)
- Make sure Dyndle features are loaded in the application
- Add configuration (see [Configuration Reference](configuration.html))

## Install

NuGet package: `Dyndle.Modules.Core`

NuGet package: `Dyndle.Providers.XXX` - depending on the Tridion version you are using

## Wire up the application

If you already have a DI setup in place or you would like to use a framework that is not supported by quickstart, you will make sure that Dyndle is properly initialized before and after DI container is built. If you already have existing functionality in place, make sure that it is modified like you find in the examples below.

1. Pre-application start:

   Execute `Dyndle.Modules.Core.Bootstrap.Run()`

1. Register all controllers in Bootstrap.GetControllerAssemblies():

   Syntax depends on the DI framework of choice. For reference, below is Autofac:

   ```c#
   // register all controllers referenced in the Dyndle.ControllerNamespaces appSetting
   // notes:
   // - you can add multiple namespaces, comma-separated
   // - you only need to include a part of the namespace, e.g. if your controllers are in Acme.Web.Controllers, you can also configure them as 'Acme.Web'
   // - don't forget to add your own controllers to this appSetting too
   foreach (var controllerAssembly in Dyndle.Modules.Core.Bootstrap.GetControllerAssemblies())
   {
       builder.RegisterControllers(controllerAssembly);
   }
   ```

1. Register all services from Dyndle Bootstrap. These contain class definitions from all Dyndle modules. Syntax depends on the DI framework of choice. For reference, below is Autofac:

   ```c#
   builder.Populate(Dyndle.Modules.Core.Bootstrap.ServiceCollection);
   ```

1. Register routes (post-application start):

   ```c#
   // set the default routes for Dyndle (e.g. the PageController, BinaryController, etc)
   Dyndle.Modules.Core.RouteConfig.RegisterRoutes(RouteTable.Routes);
   ```

1. Load viewmodels from both Dyndle and your assemblies (post-application start):

   ```c#
   // Register View Models
   var viewModelFactory = DependencyResolver.Current.GetService<IViewModelFactory>();
   viewModelFactory?.LoadViewModels(Bootstrap.GetViewModelAssemblies());
   ```

### Register Area

Add `CoreAreaRegistration` class that will register an MVC area that has access to Dyndle controllers. By using `BaseModuleAreaRegistration`, area context is prepopulated with a `Dyndle.Modules.Core.Controllers` namespace, that contains: page, region, entity and binary controllers.

```c#
public class CoreAreaRegistration : BaseModuleAreaRegistration
{
    public override string AreaName => "Core";
}
```

For this reason, views to be used by these controllers should be placed in `Areas\Core\`. For instance, if the viewname for a page template is set to `EventsPage`. The page controller that is registered in `Core` area will look for the view in `Areas\Core\Page\EventsPage.cshtml`.

_**Please note**: by default, all views, controllers and actions will be located in the Core area. This means that views are expected to be located in Areas\Core\Views\Page (for page views) and Areas\Core\Views\Entity (for entity views). You can also create your own areas. In that case, you need to add a metadata field to the templates, called 'area', and give it the appropriate value.
