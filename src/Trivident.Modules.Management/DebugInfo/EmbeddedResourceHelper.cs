﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Trivident.Modules.Core.DebugInfo
{
    /// <summary>
    /// Helper functions to manipulate embedded resources
    /// </summary>
    public static class EmbeddedResourceHelper
    {

        internal static readonly string DEBUG_INFO_RESOURCE_BASE_PATH = "Trivident.Modules.Management.DebugInfo.Resources.";

        public static string GetResourceAsString(string resourceFile)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string result = string.Empty;
            using (Stream stream = assembly.GetManifestResourceStream(DEBUG_INFO_RESOURCE_BASE_PATH + resourceFile))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }
    }
}