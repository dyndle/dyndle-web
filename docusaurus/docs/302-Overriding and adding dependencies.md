---
id: dependencies
title: Overriding / adding dependencies
sidebar_label: Overriding / adding dependencies
---

Dyndle comes with a lot of interfaces and implementing classes which need to be registered with the dependency injection framework of your choice. This is normally done either in the Global.asax.cs or, if you are using the Dyndle Quickstart, in the StartDyndle.cs file in the App_Start folder of your web application.

In either case, there will be a line where the ServiceContainer in the Dyndle Bootstrap class is used to populate the DI-container. In StartDyndle, this is what this looks like:

```c#
builder.Populate(Bootstrap.ServiceCollection);
builder.UseDD4T();
var container = builder.Build();
```

The line 'builder.Populate(Bootstrap.ServiceCollection)' tells the DI-framework to set all the necessary dependencies for Dyndle. The next line, 'builder.UseDD4T()' does the same for DD4T.
You may want to override any of the Dyndle or DD4T implementations. In that case, you need to add your own registrations BEFORE you populate the builder with the Dyndle and DD4T settings. For example, let's say you have overridden the DD4T ViewModelFactory. You would then add something like this:

```c#
// overriding a DD4T or Dyndle implementation
builder.RegisterType<MyViewModelFactory>().As<IViewModelFactory>().SingleInstance();
// populate the builder
builder.Populate(Bootstrap.ServiceCollection);
// etc...
```

This code is how you would do it with Autofac, but other DI-frameworks work in a very similar way.

It is also possible to create completely new interfaces and implementing classes. You configure these in the same way, e.g.:

```c#
// overriding a DD4T or Dyndle implementation
builder.RegisterType<MyViewModelFactory>().As<IViewModelFactory>().SingleInstance();
// configure your own implementations
builder.RegisterType<MySpecialService>().As<ISpecialService>().SingleInstance();
// populate the builder
builder.Populate(Bootstrap.ServiceCollection);
// etc...
```

_**Please note**: you can also register types in a service collection per MVC area, as described in the [following chapter](adding-routes#register-types)_
