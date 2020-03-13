﻿using System.Web.Mvc;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Personalization.Contracts;
using Trivident.Modules.Personalization.Providers;

namespace Trivident.Modules.Personalization
{
	public class PersonalizationAreaRegistration : BaseModuleAreaRegistration
	{
		public override string AreaName
		{
			get { return "Personalization"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			base.RegisterArea(context);
		}

		public override void RegisterTypes(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton(typeof(IPersonalizationProvider), typeof(BasePersonalizationProvider));
			serviceCollection.AddSingleton(typeof(IPersonalizationDataStore), typeof(CookieDataStore));
			serviceCollection.AddSingleton(typeof(IWebPageEnrichmentProvider), typeof(TargetGroupPageEnrichmentProvider));
		}
	}
}