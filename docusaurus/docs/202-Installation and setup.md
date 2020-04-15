---
id: installation
title: Installing Dyndle without the QuickStart
sidebar_label: Setting up web application
---

In order to use Dyndle, following steps must be carried out:

- Install NuGet package(s)
- Make sure Dyndle features are loaded in the application
- Add configuration (see [next chapter](configuration))
- Make sure there all neccessary items are present in Tridion (see [Dyndle CLI](cli)).

## Install

NuGet package: `Dyndle.Modules.Core`

NuGet package: `Dyndle.Providers.XXX` - depending on the Tridion version you are using

## Load

We do recommend that you first give our [quickstart](getting-started) a go. Using Quickstart NuGet package will add classes and dependencies neccessary to load Dyndle.

### Do It Yourself

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

_**Please note**: above mentions default values that in our experience suffice for a lot of scenarios. However, each component and page template can have all these values set on its metadata for: area, controller, action, view name._
