using System;
using System.Linq;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Core.Contracts.Resolvers;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Defaults;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Exceptions;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Models.System;

namespace Dyndle.Modules.Core.Factories
{
    /// <summary>
    ///     Class ViewModelFactory.
    ///     Overrides <see cref="DD4T.ViewModels.ViewModelFactory" /> to allow for some improvements like specific exception
    ///     handling..
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.ViewModelFactory" />
    public class ViewModelFactory : DD4T.ViewModels.ViewModelFactory
    {
        private readonly IDD4TConfiguration _configuration;
        private readonly IViewModelKeyProvider _keyProvider;
        private readonly ILogger _logger;
        private readonly IViewModelResolver _resolver;

        /// <summary>
        ///     New View Model Builder
        /// </summary>
        /// <param name="keyProvider">A View Model Key provider</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="linkResolver">The link resolver.</param>
        /// <param name="richTextResolver">The rich text resolver.</param>
        /// <param name="contextResolver">The context resolver.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public ViewModelFactory(IViewModelKeyProvider keyProvider,
            IViewModelResolver resolver,
            ILinkResolver linkResolver,
            IRichTextResolver richTextResolver,
            IContextResolver contextResolver,
            IDD4TConfiguration configuration,
            ILogger logger
        )
            : base(keyProvider, resolver, linkResolver, richTextResolver, contextResolver, configuration, logger)
        {
            logger.ThrowIfNull(nameof(logger));
            resolver.ThrowIfNull(nameof(resolver));
            keyProvider.ThrowIfNull(nameof(keyProvider));
            configuration.ThrowIfNull(nameof(configuration));

            _resolver = resolver;
            _keyProvider = keyProvider;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        ///     Builds the view model by attribute.
        ///     Overridden to create ExceptionEntities to allow for a friendly display of partial exceptions and logging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelData">The model data.</param>
        /// <param name="contextModel">The context model.</param>
        /// <returns>IViewModel.</returns>
        public override IViewModel
            BuildViewModelByAttribute<T>(IModel modelData,
                IContextModel contextModel = null) //where T : IModelAttribute 
        {

            IViewModel result = null;
            Type type = null;

            if (modelData is IPage page && page?.PageTemplate?.MetadataFields != null)
            {
                if (page.PageTemplate.MetadataFields.ContainsKey("PreferredModelType"))
                {
                    type = Type.GetType(page.PageTemplate.MetadataFields["PreferredModelType"].Value, false);
                }
            }

            if (type == null)
            {
                try
                {
                    type = FindViewModelByAttribute<T>(modelData);
                }
                catch (Exception ex)
                {
                    if (modelData is IPage)
                    {
                        throw;
                    }

                    _logger.Error(ex.ToString());
                    return new ExceptionEntity(ex, modelData);
                }
            }

            if (type == null) return null;

            _logger.Debug("Building ViewModel based on type " + type.FullName);
            result = BuildViewModel(type, modelData, contextModel);
            return result;
        }

        /// <summary>
        ///     Finds the view model by attribute.
        ///     Overridden to order models based on most specific match using ViewModelKeys
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <param name="typesToSearch">The types to search.</param>
        /// <returns>Type.</returns>
        public override Type FindViewModelByAttribute<T>(IModel data, Type[] typesToSearch = null)
        {
            //Anyway to speed this up? Better than just a straight up loop?
            _logger.Debug($"started FindViewModelByAttribute {(typesToSearch == null ? "without" : "with")} types to search in");
            typesToSearch = typesToSearch ?? ViewModels.ToArray();
            var typesList = typesToSearch.Select(a => new {Attribute = _resolver.GetCustomAttribute<T>(a), Type = a})
                .Where(a => a.Attribute != null);
            _logger.Debug($"using {typesToSearch.Count()} typesToSearch");
            // _logger.Debug($"searching for one of: {string.Join(",", typesToSearch.Select(t => t.FullName))}");

            typesList = typesList.OrderByDescending(t => t.Attribute.ViewModelKeys?.Count() ?? 0);

            var key = _keyProvider.GetViewModelKey(data);
            _logger.Debug("found key " + key);

            // first, check only for models that have one ore more ViewModelKeys
            int i = 0;
            foreach (var typeref in typesList)
            {
                i++;
                var type = typeref.Type;
                var modelAttr = typeref.Attribute;

                if (modelAttr == null) continue;

                if (!modelAttr.IsMatch(data, key)) continue;

                _logger.Debug($"returning type {type.FullName} after examining {i} model classes");
                return type;
            }

            if (_configuration.UseDefaultViewModels)
            {
                if (data is IPage)
                {
                    // TODO: make default page model configurable
                    _logger.Debug("no viewmodel found, using default page viewmodel " + typeof(DefaultPage).FullName);
                    return typeof(DefaultPage);
                }
                if (data is IComponentPresentation)
                {                   
                    _logger.Debug("no viewmodel found, using default entity viewmodel");
                    return DefaultEntityType;
                }
            }
            var e = new ViewModelNotFoundException(data);
            _logger.Warning($"Could not find a valid ViewModel for item {e.Identifier}");

            throw e;
        }

        private static Type _defaultEntityType;
        private static Type DefaultEntityType
        {
            get
            {
                if (_defaultEntityType == null)
                {
                    var defaultEntityTypeName = DyndleConfig.DefaultEntityTypeName;
                    _defaultEntityType = Bootstrap.LoadedAssemblies.SelectMany(a => a.DefinedTypes).FirstOrDefault(t => t.Name.Equals(defaultEntityTypeName, StringComparison.InvariantCultureIgnoreCase));
                }
                return _defaultEntityType;
            }
        }
    }
}