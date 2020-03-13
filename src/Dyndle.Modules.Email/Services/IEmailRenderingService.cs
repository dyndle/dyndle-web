using System;

namespace Dyndle.Modules.Email.Services
{
    /// <summary>
    /// Interface IEmailRenderingService
    /// Used to describe an service that can render email content using Tridion pages
    /// </summary>
    public interface IEmailRenderingService
    {
        /// <summary>
        /// Renders the email body based on a page in Tridion.
        /// </summary>
        /// <param name="localUri">The local URI to render.</param>
        /// <param name="routeValues">The route values to use during rendering of the actions on the page and also used for placeholders.</param>
        /// <returns>System.String. Full page rendered and ready to send as email</returns>
        string RenderEmailBody(Uri localUri, object routeValues);

        /// <summary>
        /// Renders the email body based on a page in Tridion.
        /// </summary>
        /// <param name="itemId">The component identifier used to determine the url of the page.</param>
        /// <param name="routeValues">The route values to use during rendering of the actions on the page and also used for placeholders.</param>
        /// <returns>System.String. Full page rendered and ready to send as email</returns>
        /// <exception cref="System.ArgumentException"></exception>
        string RenderEmailBody(int itemId, object routeValues);
    }
}