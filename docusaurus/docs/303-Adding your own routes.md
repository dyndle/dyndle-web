---
id: adding-routes
title: Adding your own routes
sidebar_label: Adding your own routes
---

If you use Dyndle, you still own the web application and it is up to you to set up the routes in any way you would like. However, if you want to use Dyndle MVC features such as controllers and model binders, you need to set up an area as explained [here](installation#register-area).

Next to the AreaName that must be overriden, `BaseModuleAreaRegistration` also defines two virtual methods that may be overriden: `RegisterRoutes` and `RegisterTypes`. Both of these methods are called from [`RegisterArea`](https://docs.microsoft.com/en-us/dotnet/api/system.web.mvc.arearegistration.registerarea?view=aspnet-mvc-5.2).

## Register Routes

In the first method you may define routes that will be registered within this MVC area. This gives you granular control over routing if you have multiple areas set up. Normal Asp.Net MVC syntax applies for route registration. Example below features an example from Dyndle Image Enhancement module, where also an MVC constrain is used:

```c#
public override void RegisterRoutes(AreaRegistrationContext context)
{
	context.MapRoute(
		AreaName + "_EnhancedBinaries",
		"{*url}",
		new {controller = "ImageEnhancement", action = "EnhanceImage"},

		new {page = new EnhancedImageConstraint()});

	base.RegisterRoutes(context);
}
```

