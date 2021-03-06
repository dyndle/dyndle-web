﻿//using DD4T.ContentModel.Contracts.Resolvers;
//using DD4T.Utils.Resolver;
//using Microsoft.Extensions.DependencyInjection;
//using Dyndle.Modules.Core.Contracts;
//using Dyndle.Modules.Core.Interfaces;
//using Dyndle.Modules.Core.Resolvers;

//namespace Dyndle.Modules.Core.Modules
//{
//    /// <summary>
//    /// Register types for Resolvers
//    /// </summary>
//    public class ResolversModule : IServiceCollectionModule
//    {
//	    private readonly bool _isDevMode;

//	    public ResolversModule(bool isDevMode)
//	    {
//		    _isDevMode = isDevMode;
//	    }

//	    public void RegisterTypes(IServiceCollection serviceCollection)
//	    {
//		    if (_isDevMode)
//		    {
//			    serviceCollection.AddSingleton(typeof(IPublicationResolver), typeof(DefaultPublicationResolver));
//			    serviceCollection.AddSingleton(typeof(IExtendedPublicationResolver), typeof(DevPublicationResolver));
//		    }
//		    else
//		    {
//				serviceCollection.AddSingleton(typeof(IPublicationResolver), typeof(PublicationResolver));
//			    serviceCollection.AddSingleton(typeof(IExtendedPublicationResolver), typeof(PublicationResolver));
//			}
//		}
//    }
//}