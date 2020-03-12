using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// Class ECLAttributeBase.
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Attributes.ComponentAttributeBase" />
    public abstract class ECLAttributeBase : ComponentAttributeBase
    {
        /// <summary>
        /// Loads the extension data.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns>IFieldSet.</returns>
        protected IFieldSet LoadExtensionData(IComponent component)
        {
            if (component.ExtensionData.IsNull())
                return null;

            IFieldSet extensionMeta;
            if (component.ExtensionData.TryGetValue(CoreConstants.General.ECLExtensionMeta, out extensionMeta))
            {
                return extensionMeta;
            }
            return null;
        }
    }

    /// <summary>
    /// Class TextMetaEcl.
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Attributes.ViewModels.ECLAttributeBase" />
    public class TextMetaEcl : ECLAttributeBase
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets the expected type of the return.
        /// </summary>
        /// <value>The expected type of the return.</value>
        public override Type ExpectedReturnType
        {
            get
            {
                return typeof(string);
            }
        }

        /// <summary>
        /// Gets the property values.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="property">The property.</param>
        /// <param name="template">The template.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>IEnumerable.</returns>
        public override IEnumerable GetPropertyValues(IComponent component, IModelProperty property, ITemplate template, IViewModelFactory factory)
        {
            var extensionMeta = base.LoadExtensionData(component);

            if (this.FieldName.IsNullOrEmpty())
                this.FieldName = property.Name;

            var value = extensionMeta?[this.FieldName]?.Value;

            return new[] { value };
        }
    }

    /// <summary>
    /// Class NumericMetaEcl.
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Attributes.ViewModels.ECLAttributeBase" />
    public class NumericMetaEcl : ECLAttributeBase
    {
        /// <summary>
        /// Gets or sets the name of the field.
        /// </summary>
        /// <value>The name of the field.</value>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets the expected type of the return.
        /// </summary>
        /// <value>The expected type of the return.</value>
        public override Type ExpectedReturnType
        {
            get
            {
                return typeof(int);
            }
        }

        /// <summary>
        /// Gets the property values.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="property">The property.</param>
        /// <param name="template">The template.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>IEnumerable.</returns>
        public override IEnumerable GetPropertyValues(IComponent component, IModelProperty property, ITemplate template, IViewModelFactory factory)
        {
            var extensionMeta = base.LoadExtensionData(component);

            if (this.FieldName.IsNullOrEmpty())
                this.FieldName = property.Name;

            var value = extensionMeta?[this.FieldName]?.NumericValues.FirstOrDefault().ToString();
            int result;

            if (int.TryParse(value, out result))
                return new[] { result };

            return null;
        }
    }

    /// <summary>
    /// Class CDNPathsEcl.
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Attributes.ViewModels.ECLAttributeBase" />
    public class CDNPathsEcl : ECLAttributeBase
    {
        /// <summary>
        /// Gets the expected type of the return.
        /// </summary>
        /// <value>The expected type of the return.</value>
        public override Type ExpectedReturnType
        {
            get
            {
                return typeof(Dictionary<string, string>);
            }
        }

        /// <summary>
        /// Gets the property values.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="property">The property.</param>
        /// <param name="template">The template.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>IEnumerable.</returns>
        public override IEnumerable GetPropertyValues(IComponent component, IModelProperty property, ITemplate template, IViewModelFactory factory)
        {
            var extensionMeta = LoadExtensionData(component);

            var value = extensionMeta?[CoreConstants.General.CDNPathKey]?.Value;

            if (value == null)
            {
                return null;
            }

            var listenValue = value.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

            if (listenValue.IsNull())
            {
                return null;
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in listenValue.Where(s => !s.IsNullOrEmpty()))
            {
                var fileName = item.Split(':').First();

                if (!dictionary.ContainsKey(fileName))
                {
                    dictionary[fileName] = string.Join(":", item.Split(':').Skip(1));
                }
            }

            return new[] { dictionary };
        }
    }
}