using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Dyndle.Modules.Core.Json
{
    /// <summary>
    /// Custom implementation of System.Web.Mvc.JsonResult. Ensures JSON is camelCased and null elements are ommitted.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.JsonResult" />
    public class CustomJsonResult : JsonResult
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter> { new StringEnumConverter() },
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Execute the result
        /// </summary>
        /// <param name="context">The current controller context</param>
        /// <exception cref="InvalidOperationException">GET request not allowed</exception>
        public override void ExecuteResult(ControllerContext context)
        {
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("GET request not allowed");
            }
            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/json";
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data == null)
            {
                return;
            }
            response.Write(JsonConvert.SerializeObject(this.Data, Settings));
        }
    }
}
