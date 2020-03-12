using DD4T.ViewModels.Attributes;
using DD4T.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Models.System
{
    /// <summary>
    /// Class SystemSettings.
    /// Holds the data from the System Settings in Tridion
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Base.ViewModelBase" />
    [ContentModel("system-settings", true)]
    public class SystemSettings : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the key value pairs.
        /// </summary>
        /// <value>The key value pairs.</value>
        [EmbeddedSchemaField(FieldName = "settings", EmbeddedModelType = typeof(KeyValueModel))]
        public List<KeyValueModel> KeyValuePairs { get; set; } = new List<KeyValueModel>();
    }

    /// <summary>
    /// Class KeyValueModel.
    /// Used to hold key value data from Tridion schema's
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Base.ViewModelBase" />
    [ContentModel("key-value", true)]
    public class KeyValueModel : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [TextField]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [TextField]
        public string Value { get; set; }
    }
}