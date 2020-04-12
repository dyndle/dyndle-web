using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using DD4T.Core.Contracts.ViewModels;
using DD4T.DI.Autofac;
using Bootstrap = Dyndle.Modules.Core.Bootstrap;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TheBestCompany.Website.StartDyndle), "PreStart")]
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(TheBestCompany.Website.StartDyndle), "PostStart")]

namespace TheBestCompany.Website
{
    public static class StartDyndle
    {
        private static ContainerBuilder builder;
        public static void PreStart()
        {
            builder = new ContainerBuilder();
            Bootstrap.Run();
        }

        public static void PostStart()
        {

            // register all controllers referenced in the Dyndle.ControllerNamespaces appSetting
            // notes:
            // - you can add multiple namespaces, comma-separated
            // - you only need to include a part of the namespace, e.g. if your controllers are in Acme.Web.Controllers, you can also configure them as 'Acme.Web'
            // - don't forget to add your own controllers to this appSetting too
            foreach (var controllerAssembly in Bootstrap.GetControllerAssemblies())
            {
                builder.RegisterControllers(controllerAssembly);
            }

            builder.RegisterFilterProvider();
            builder.Populate(Bootstrap.ServiceCollection);

            // if you want to override any of the types from DD4T or Dyndle, do it below:
            //builder.RegisterType<MyViewModelFactory>().As<IViewModelFactory>().SingleInstance();

            builder.UseDD4T();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // set the default routes for Dyndle (e.g. the PageController, BinaryController, etc)
            Dyndle.Modules.Core.RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Register View Models
            var viewModelFactory = DependencyResolver.Current.GetService<IViewModelFactory>();
            viewModelFactory?.LoadViewModels(Bootstrap.GetViewModelAssemblies());
        }
    }
}