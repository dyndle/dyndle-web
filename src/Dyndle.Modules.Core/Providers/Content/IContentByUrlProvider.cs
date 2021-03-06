﻿using System;
using DD4T.Core.Contracts.ViewModels;

namespace Dyndle.Modules.Core.Providers.Content
{
    /// <summary>
    /// interface for content providers by url
    /// load viewmodel object based on url from Tridion.
    /// </summary>
    /// <summary>
    /// Interface IContentByUrlProvider
    /// </summary>
    public interface IContentByUrlProvider
    {
        /// <summary>
        /// load Tridion page and create a viewmodel based on url.
        /// </summary>
        /// <param name="urlOrTcmUri">The URL or TCM URI.</param>
        /// <param name="preferredModelType">Type of the preferred model.</param>
        /// <returns>IViewModel.</returns>
        IViewModel Retrieve(string urlOrTcmUri, Type preferredModelType = null);
    }
}