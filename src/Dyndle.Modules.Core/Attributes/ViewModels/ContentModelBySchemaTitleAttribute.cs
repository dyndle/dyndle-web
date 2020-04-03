using System;
using System.Linq;
using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;

namespace Dyndle.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// An Attribute for identifying a Content View Model. Can be based on either Embedded Schema or Component Schema
    /// </summary>
    public class ContentModelBySchemaTitleAttribute : Attribute, IContentModelAttribute
    {
        //TODO: De-couple this from the Schema name specifically? What would make sense?
        //TOOD: Possibly change this to use purely ViewModelKey and make that an object, leave it to the key provider to assign objects with logical equals overrides

        private readonly string schemaRootElementName;
        private bool inlineEditable = false;
        private readonly bool isDefault = false;
        private string[] viewModelKeys;

        /// <summary>
        /// View Model
        /// </summary>
        /// <param name="schemaRootElementName">Tridion schema name for component type for this View Model</param>
        /// <param name="isDefault">Is this the default View Model for this schema. If true, Components
        /// with this schema will use this class if no other View Models' Keys match.</param>
        public ContentModelBySchemaTitleAttribute(string schemaRootElementName, bool isDefault)
        {
            this.schemaRootElementName = schemaRootElementName;
            this.isDefault = isDefault;
        }
        /// <summary>
        ///Using Schema Name ties each View Model to a single Tridion Schema -- this is probably ok in 99% of cases
        ///Using schema name doesn't allow us to de-couple the Model itself from Tridion however (neither does requiring
        ///inheritance of IViewModel!)
        ///Possible failure: if the same model was meant to represent similar parts of multiple schemas (should however
        ///be covered by decent Schema design i.e. use of Embedded Schemas and Linked Components. Same fields shouldn't
        ///occur repeatedly)
        /// </summary>
        public string SchemaRootElementName
        {
            get
            {
                return schemaRootElementName;
            }
        }

        /// <summary>
        /// Identifiers for further specifying which View Model to use for different presentations.
        /// </summary>
        public string[] ViewModelKeys
        {
            get { return viewModelKeys; }
            set { viewModelKeys = value; }
        }

        /// <summary>
        /// Is inline editable. Only for semantic use.
        /// </summary>
        public bool InlineEditable
        {
            get
            {
                return inlineEditable;
            }
            set
            {
                inlineEditable = value;
            }
        }

        /// <summary>
        /// Is the default View Model for the schema. If set to true, this will be the View Model to use for a given schema if no View Model ID is specified.
        /// </summary>
        public bool IsDefault { get { return isDefault; } }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (ViewModelKeys != null ? ViewModelKeys.GetHashCode() : 0) * 37 + SchemaRootElementName.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is ContentModelAttribute key)
            {
                if (this.ViewModelKeys != null && key.ViewModelKeys != null)
                {
                    //if both have a ViewModelKey set, use both ViewModelKey and schema
                    //Check for a match anywhere in both lists
                    var match = from i in this.ViewModelKeys.Select(a => a.ToLower())
                                join j in key.ViewModelKeys.Select(a => a.ToLower())
                                on i equals j
                                select i;
                    //Schema names match and there is a matching view model ID
                    if (this.SchemaRootElementName.Equals(key.SchemaRootElementName, StringComparison.OrdinalIgnoreCase)
                        && match.Count() > 0)
                        return true;
                }
                //Note: if the parent of a linked component is using a View Model Key, the View Model
                //for that linked component must either be Default with no View Model Keys, or it must
                //have the View Model Key of the parent View Model
                if (((this.ViewModelKeys == null || this.ViewModelKeys.Length == 0) && key.IsDefault) //this set of IDs is empty and the input is default
                    || ((key.ViewModelKeys == null || key.ViewModelKeys.Length == 0) && this.IsDefault)) //input set of IDs is empty and this is default
                //if (key.IsDefault || this.IsDefault) //Fall back to default if the view model key isn't found -- useful for linked components
                {
                    //Just compare the schema names
                    return this.SchemaRootElementName.Equals(key.SchemaRootElementName, StringComparison.OrdinalIgnoreCase);
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether the specified data is match.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified data is match; otherwise, <c>false</c>.
        /// </returns>
        public bool IsMatch(IModel data, string key)
        {
            bool result = false;
            string schemaTitle = null;
            if (data != null)
            {
                //Ideally we'd have a common interface for these 2 that have a Schema property
                if (data is IComponentPresentation definedData2)
                {
                    //schemaRootElementName = definedData.Component.Multimedia == null ? definedData.Component.Schema.RootElementName : definedData.Component.Schema.Title;
                    schemaTitle = definedData2.Component.Schema.Title;
                }
                else if (data is IComponent definedData1)
                {
                    //schemaRootElementName = definedData.Multimedia == null ? definedData.Schema.RootElementName : definedData.Schema.Title;
                    schemaTitle = definedData1.Schema.Title;
                }
                else if (data is IEmbeddedFields definedData)
                {
                    schemaTitle = definedData.EmbeddedSchema.Title;
                    //schemaRootElementName = definedData.EmbeddedSchema.RootElementName;
                }
                if (!String.IsNullOrEmpty(schemaTitle))
                {
                    var compare = new ContentModelAttribute(schemaTitle, false)
                    {
                        ViewModelKeys = key == null ? null : new string[] { key }
                    };
                    result = this.Equals(compare);
                }
            }
            return result;
        }
    }
}