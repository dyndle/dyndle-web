---
id: dependency-framework-examples
title: DI frameworks implementation examples
description: Documentation for Dyndle
sidebar_label: DI frameworks implementation examples
---

The following paragraphs contain example implementations of dependency injection frameworks, other than the default Autofac DI, supported by Dyndle and DD4T. Currently it consists of Ninject and Simple Injector, Unity is not yet supported by Dyndle but coming soon. 

### Ninject

#### Installation

Install DD4T.DI.Ninject nuget package. This will also install the Ninject package as a dependency. Then also install the Ninject.MVC5 package, this will handle controller registrations for you. This package will also install Ninject.Web.Common.WebHost and Ninject.Web.Common as it's dependencies.

#### Implementation

The installation of the packages will have added two classes to you App_Start folder. `NinjectRegistration.cs` and `ServiceProviderNinjectModule.cs`. The `NinjectRegistration` class will call the `NinjectModule` which will register the services from Dyndle. It should look like this.

```c#
namespace Microsoft.Framework.DependencyInjection.Ninject
{
    public static class NinjectRegistration
    {
        public static void Populate(this IKernel kernel, IEnumerable<ServiceDescriptor> descriptors)
        {
            kernel.Load(new ServiceProviderNinjectModule(descriptors));
        }
    }
}
```

The `NinjectModule` can be used to register all of the Dyndle services, but should contain logic to overwrite any bindings that may already exist in DD4T, unlike other DI frameworks Ninject does not handle this for you. The final `NinjectModule`  should look something like this.

```c#
namespace Microsoft.Framework.DependencyInjection.Ninject
{
    internal class ServiceProviderNinjectModule : NinjectModule
    {
        private readonly IEnumerable<ServiceDescriptor> _serviceDescriptors;

        public ServiceProviderNinjectModule(
                IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            _serviceDescriptors = serviceDescriptors;
        }

        public override void Load()
        {
            foreach (var descriptor in _serviceDescriptors)
            {
                IBindingWhenInNamedWithOrOnSyntax<object> binding;

                if (descriptor.ImplementationType != null)
                {
                    var existingbindings = Kernel.GetBindings(descriptor.ServiceType);

                    if (existingbindings.Any(b => b.Metadata.Get<Type>("implementationType") == null))
                    {
                        binding = (IBindingWhenInNamedWithOrOnSyntax<object>)Rebind(descriptor.ServiceType).To(descriptor.ImplementationType).WithMetadata("implementationType", descriptor.ImplementationType);
                        binding.InSingletonScope();
                    }
                    else if (!existingbindings.Any(b => b.Metadata.Get<Type>("implementationType") == descriptor.ImplementationType))
                    {
                        binding = (IBindingWhenInNamedWithOrOnSyntax<object>)Bind(descriptor.ServiceType).To(descriptor.ImplementationType).WithMetadata("implementationType", descriptor.ImplementationType);
                        binding.InSingletonScope();
                    }
                }
            }
        }
    }
}
```

Now that we have the module ready we need to update our `Global.asax` to inherit from `NinjectHttpApplication`, this will handle the registration of controllers for us. Then we define a `CreateKernel` method which will be called by the inherited class. It should return a `IKernel` object, which is an interface from the Ninject framework. In this method we first call the `Bootstrap.Run()` method from Dyndle Core to use it's servicecollection later. Then we register areas and routes with `AreaRegistration.RegisterAllAreas()` and ` RouteConfig.RegisterRoutes(RouteTable.Routes)`. Then we create a new Ninject`StandardKernel` and call the `UseDD4T()` method from the `DD4T.DI.Ninject` module. Next we can call the `NinjectRegistration` class to register the servicecollection from Dyndle. `NinjectRegistration.Populate(kernel, Bootstrap.ServiceCollection)` 

Note that we need to do this after we call `UseDD4T()` so our Ninject module can properly overwrite it's bindings. 

Then we can register the Dyndle routes and load the viewmodels.

```c#
Dyndle.Modules.Core.RouteConfig.RegisterRoutes(RouteTable.Routes);
var viewModelFactory = DependencyResolver.Current.GetService<IViewModelFactory>();
viewModelFactory?.LoadViewModels(Bootstrap.GetViewModelAssemblies());
```

Now we can return the kernel with all of the bindings we need and Ninject will handle the rest. The final class should look something like this.

```c#
 public class MvcApplication : NinjectHttpApplication
    {
        protected override IKernel CreateKernel()
        {
            Bootstrap.Run();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var kernel = new StandardKernel();

            kernel.UseDD4T();

            NinjectRegistration.Populate(kernel, Bootstrap.ServiceCollection);

            Dyndle.Modules.Core.RouteConfig.RegisterRoutes(RouteTable.Routes);

            var viewModelFactory = DependencyResolver.Current.GetService<IViewModelFactory>();
            viewModelFactory?.LoadViewModels(Bootstrap.GetViewModelAssemblies());

            return kernel;
        }
    }
```



### Simple injector

#### Installation

Install `DD4T.DI.SimpleInjector`, this will install `SimpleInjector`, `SimpleInjector.Integration.Web` and `SimpleInjector.Integration.Web.Mvc` as it's package dependencies. 

#### Implementation

Unlike Ninject, Simple injector can use the `CoreApplicationBase` from the Dyndle Core module. We can write a `ApplicationBase` class that inherits from it and implements a `GetDependencyResolver` method which will provide it with our SimpleInjector dependency resolver. The first thing we need to do in this method is instantiate a new `Container` from the SimpleInjector framework. Then we configure the container's lifecycle with the following statement `container.Options.DefaultScopedLifestyle = new WebRequestLifestyle()`. Next, we can register the mvc providers and controllers like so. 

```c#
container.RegisterCollection<IWebPageEnrichmentProvider>(new Assembly[] { Bootstrap.GetExecutingAssembly });
container.RegisterMvcControllers(Bootstrap.GetExecutingAssembly);
container.RegisterMvcIntegratedFilterProvider();
```

 Now we have to register the Dyndle services, we can iterate over the `serviceCollection` parameter of the `GetDependencyResolver` method which contains all of the Dyndle services from the Dyndle Core Bootstrap class. 

```c#
foreach (var serviceDescriptor in serviceCollection)
            {
                if (serviceDescriptor.ServiceType == typeof(IContentByUrlProvider) &&
                    serviceDescriptor.ImplementationType == typeof(DefaultContentByUrlProvider))
                {
                    container.RegisterCollection<IContentByUrlProvider>(new[] { typeof(DefaultContentByUrlProvider) });
                    continue;
                }

                container.RegisterType(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType,
                    ConvertTo(serviceDescriptor.Lifetime));
            }
```

Note that there is one conditional statement in here for the `DefaultContentByUrlProvider` service. It registers the service in a collection to match the target controller arguments and is necessary for it to function. 

There is also a call to a `ConvertTo` method to transform the lifetime of a service to a `LifeStyle` object. It is defined as follows.

```c#
private Lifestyle ConvertTo(ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Transient:
                    return Lifestyle.Transient;

                case ServiceLifetime.Scoped:
                    return Lifestyle.Scoped;

                default:
                    return Lifestyle.Singleton;
            }
        }
```

We have now registered all of the Dyndle services and can move on to the DD4T services. We register them by calling `UseDD4T()` on the container. Then we can call `Verify()` as well to make sure everything was registered correctly. Now we can register the Dyndle Routes and load the Viewmodels.

```c#
Dyndle.Modules.Core.RouteConfig.RegisterRoutes(RouteTable.Routes);
var viewModelFactory = DependencyResolver.Current.GetService<IViewModelFactory>();
viewModelFactory?.LoadViewModels(Bootstrap.GetViewModelAssemblies());
```

Then we can return the container to the `CoreApplicationBase` in a new `SimpleInjectorDependencyResolver`.

Finally we need to implement a `OnApplicationStarting` Method to register areas and routes before the Dyndle Core Bootstrap runs.

```c#
protected override void OnApplicationStarting()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
```

This concludes the implementation of the `ApplicationBase` class the end result should look something like this.

```c#
 public class ApplicationBase : CoreApplicationBase
    {
        protected override void OnApplicationStarting()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected override IDependencyResolver GetDependencyResolver(IServiceCollection serviceCollection)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.RegisterCollection<IWebPageEnrichmentProvider>(new Assembly[] { Bootstrap.GetExecutingAssembly });
            container.RegisterMvcControllers(Bootstrap.GetExecutingAssembly);
            container.RegisterMvcIntegratedFilterProvider();

            foreach (var serviceDescriptor in serviceCollection)
            {
                if (serviceDescriptor.ServiceType == typeof(IContentByUrlProvider) &&
                    serviceDescriptor.ImplementationType == typeof(DefaultContentByUrlProvider))
                {
                    container.RegisterCollection<IContentByUrlProvider>(new[] { typeof(DefaultContentByUrlProvider) });
                    continue;
                }

                container.RegisterType(serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType,
                    ConvertTo(serviceDescriptor.Lifetime));
            }

            container.UseDD4T();
            container.Verify();

            Dyndle.Modules.Core.RouteConfig.RegisterRoutes(RouteTable.Routes);
            var viewModelFactory = DependencyResolver.Current.GetService<IViewModelFactory>();
            viewModelFactory?.LoadViewModels(Bootstrap.GetViewModelAssemblies());

            return new SimpleInjectorDependencyResolver(container);
        }

        private Lifestyle ConvertTo(ServiceLifetime serviceLifetime)
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Transient:
                    return Lifestyle.Transient;

                case ServiceLifetime.Scoped:
                    return Lifestyle.Scoped;

                default:
                    return Lifestyle.Singleton;
            }
        }
    }
```

