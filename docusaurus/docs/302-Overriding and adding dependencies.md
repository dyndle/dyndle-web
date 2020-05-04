---
id: dependencies
title: Overriding / adding dependencies
sidebar_label: Overriding / adding dependencies
---

### Choosing a Dependency Injection framework ###
Dyndle uses DD4T for its Dependency Injection handling. DD4T supports the following DI frameworks:
 - Autofac
 - Ninject
 - Unity
 - SimpleInjector
 
You are free to use the DI of your choice. But be careful: if you choose the Quickstart installation, Autofac will be used!

Regardless of your choice of DI framework, you need to tell it to register the types you will be using. This is normally done either in the Global.asax.cs or, if you are using the Dyndle Quickstart, in the StartDyndle.cs file in the App_Start folder of your web application.


In either case, there will be a line where the ServiceContainer in the Dyndle Bootstrap class is used to populate the DI-container. In StartDyndle, this is what this looks like:

```c#
builder.Populate(Bootstrap.ServiceCollection);
builder.UseDD4T();
var container = builder.Build();
```


Dyndle collects all its interfaces and implementing types in the ServiceCollection. 
The line 'builder.Populate(Bootstrap.ServiceCollection)' tells the DI-framework to set all the necessary dependencies for Dyndle. The next line, 'builder.UseDD4T()' does the same for DD4T.

You may want to override any of the Dyndle or DD4T implementations. In that case, you need to add your own registrations BEFORE you populate the builder with the Dyndle and DD4T settings. For example, let's say you have overridden the DD4T ViewModelFactory. You would then add something like this:

```c#
// overriding a DD4T or Dyndle implementation
builder.RegisterType<MyViewModelFactory>().As<IViewModelFactory>().SingleInstance();
// populate the builder
builder.Populate(Bootstrap.ServiceCollection);
// etc...
```

This code is how you would do it with Autofac, but other DI-frameworks work in a very similar way (check the documentation).

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


## Registering controllers
If you create your own controllers, you can let Dyndle register them in a much simpler way. Just add the namespace of your controller to the Dyndle.ControllerNamespaces appSetting (you can separate multiple namespaces with a comma).

Any controller which lives within the specified namespace will be automatically registered and ready to go.

## Adding types to the ServiceCollection
 An alternative way to register your own types is to add them to the ServiceCollection. 

 First, find the class that overrides the BaseModuleAreaRegistration. If you are using the Quickstart, this is the CoreAreaRegistration class in the root of your web application.

 Override the RegisterType method as follows:

 ```c#
 public override void RegisterTypes(IServiceCollection serviceCollection)
{
    base.RegisterTypes(serviceCollection);
}
 ```

You can now add your types to the serviceCollection as follows:
```c#
public override void RegisterTypes(IServiceCollection serviceCollection)
{
	serviceCollection.AddSingleton(typeof(IMyType), typeof(MyType));
	serviceCollection.AddSingleton(typeof(IMyOtherType), typeof(MyOtherType));
}
```
