using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Models.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;

namespace Trivident.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// Class CustomRenderDataAttribute. Used to add more render data to the entities 
    /// like grid size and route values for entities and regions
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Attributes.ModelPropertyAttributeBase" />
    public class CustomRenderDataAttribute : ModelPropertyAttributeBase
    {
        /// <summary>
        /// Gets the MVC data from meta data on template in Tridion
        /// </summary>
        /// <param name="modelData">The model data.</param>
        /// <returns>MvcData.</returns>
        public static MvcData GetMvcData(IModel modelData)
        {
            if (modelData == null) return null;

            var isPage = false;

            ITemplate template = null;
            if (modelData is IComponentPresentation)
            {
                template = ((IComponentPresentation)modelData).ComponentTemplate;
            }
            else if (modelData is IPage)
            {
                isPage = true;
                template = ((IPage)modelData).PageTemplate;
            }

            // Run away run away
            if (template == null) return null;

            var fields = template.MetadataFields ?? new FieldSet(); // this won't break if the template has no metadata at all

            var view = Regex.Replace(template.Title, @"\[.*\]|\s", String.Empty);
            // TODO reuse logic from page rendering
            var area = "Core";
            var controller = isPage ? PageControllerName : EntityControllerName;
            var action = isPage ? PageActionName : EntityActionName;
            string[] routeValues;
            int entityGridSize = 0;

#warning configure the values
            //Todo: make it configurable
            var regionName = isPage ? null : "Region";
            var regionViewName = isPage ? null : "Region";
            string[] regionValues;
            int regionGridSize = 0;

            var areaFieldName = "area";
            var controllerFieldName = "controller";
            var actionFieldName = "action";
            var viewFieldName = "view";
            var routeValuesFieldName = "routeValues"; // TODO: Add params field on CM side and use that instead
            var entityGridSizeFieldName = "entityGridSize";
            var regionGridSizeFieldName = "regionGridSize";

            var regionNameFieldName = "region";
            var regionViewNameFieldName = "regionView";
            var regionRouteValuesFieldName = "regionRouteValues"; // TODO: Add params field on CM side and use that instead

            if (fields.ContainsKey(areaFieldName))
            {
                if (!string.IsNullOrEmpty(fields[areaFieldName].Value))
                    area = fields[areaFieldName].Value.Trim();
            }
            if (fields.ContainsKey(controllerFieldName))
            {
                if (!string.IsNullOrEmpty(fields[controllerFieldName].Value))
                    controller = fields[controllerFieldName].Value.Trim();
            }
            if (fields.ContainsKey(actionFieldName))
            {
                if (!string.IsNullOrEmpty(fields[actionFieldName].Value))
                    action = fields[actionFieldName].Value.Trim();
            }

            if (fields.ContainsKey(viewFieldName))
            {
                if (!string.IsNullOrEmpty(fields[viewFieldName].Value))
                    view = fields[viewFieldName].Value.Trim();
            }

            if (fields.ContainsKey(entityGridSizeFieldName))
            {
                if (!string.IsNullOrEmpty(fields[entityGridSizeFieldName].Value))
                    entityGridSize = int.Parse(fields[entityGridSizeFieldName].Value.Trim());
            }

            if (fields.ContainsKey(regionNameFieldName))
            {
                if (!string.IsNullOrEmpty(fields[regionNameFieldName].Value))
                    regionName = fields[regionNameFieldName].Value.Trim();
            }

            if (fields.ContainsKey(regionViewNameFieldName))
            {
                if (!string.IsNullOrEmpty(fields[regionViewNameFieldName].Value))
                    regionViewName = fields[regionViewNameFieldName].Value.Trim();
            }
          
            if (fields.ContainsKey(regionGridSizeFieldName))
            {
                if (!string.IsNullOrEmpty(fields[regionGridSizeFieldName].Value))
                    regionGridSize = int.Parse(fields[regionGridSizeFieldName].Value.Trim());
            }

            var mvcData = new MvcData();
            if (fields.ContainsKey(routeValuesFieldName))
            {
                if (!string.IsNullOrEmpty(fields[routeValuesFieldName].Value))
                {
                    routeValues = fields[routeValuesFieldName].Value.Split(',');
                    foreach (string routeValue in routeValues)
                    {
                        string[] routeValueParts = routeValue.Trim().Split(':');
                        if (routeValueParts.Length > 1 && !mvcData.RouteValues.ContainsKey(routeValueParts[0]))
                        {
                            mvcData.RouteValues.Add(routeValueParts[0], routeValueParts[1]);
                        }
                    }
                }
            }

            if (fields.ContainsKey(regionRouteValuesFieldName))
            {
                if (!string.IsNullOrEmpty(fields[regionRouteValuesFieldName].Value))
                {
                    regionValues = fields[regionRouteValuesFieldName].Value.Split(',');
                    foreach (string routeValue in regionValues)
                    {
                        string[] routeValueParts = routeValue.Trim().Split(':');
                        if (routeValueParts.Length > 1 && !mvcData.RegionRouteValues.ContainsKey(routeValueParts[0]))
                        {
                            mvcData.RegionRouteValues.Add(routeValueParts[0], routeValueParts[1]);
                        }
                    }
                }
            }

            mvcData.View = view;
            mvcData.Action = action;
            mvcData.Controller = controller;
            mvcData.Region = regionName;
            mvcData.RegionViewName = regionViewName;
            mvcData.Area = area;
            mvcData.EntityGridSize = entityGridSize;
            mvcData.RegionGridSize = regionGridSize;

            return mvcData;
        }

        private static string PageControllerName
        {
            get
            {
                return ConfigurationManager.AppSettings["DD4T.PageController"] ?? "Page";
            }
        }

        private static string PageActionName
        {
            get
            {
                return ConfigurationManager.AppSettings["DD4T.PageAction"] ?? "Page";
            }
        }

        private static string EntityControllerName
        {
            get
            {
                return (string.IsNullOrEmpty(ConfigurationManager.AppSettings["DD4T.EntityController"]) ? ConfigurationManager.AppSettings["DD4T.ComponentPresentationController"] : ConfigurationManager.AppSettings["DD4T.EntityController"]) ?? "Entity";
            }
        }

        private static string EntityActionName
        {
            get
            {
                return (string.IsNullOrEmpty(ConfigurationManager.AppSettings["DD4T.EntityAction"]) ? ConfigurationManager.AppSettings["DD4T.ComponentPresentationAction"] : ConfigurationManager.AppSettings["DD4T.EntityAction"]) ?? "Entity";
            }
        }

        /// <summary>
        /// not using the RenderDataAttribute within DD4T.Mvc; because that class has a hidden dependency on DD4TConfiguration.
        /// and that makes it hard to test.
        /// Gets the property values.
        /// </summary>
        /// <param name="modelData">The model data.</param>
        /// <param name="property">The property.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>IEnumerable.</returns>
        public override IEnumerable GetPropertyValues(IModel modelData, IModelProperty property, IViewModelFactory factory)
        {
            var mvcData = GetMvcData(modelData);
            return new[] { mvcData };
        }

        /// <summary>
        /// Gets the expected type of the return.
        /// </summary>
        /// <value>The expected type of the return.</value>
        public override Type ExpectedReturnType
        {
            get
            {
                return typeof(IMvcData);
            }
        }
    }
}