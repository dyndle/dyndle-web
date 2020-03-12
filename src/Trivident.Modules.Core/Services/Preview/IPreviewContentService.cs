using Trivident.Modules.Core.Models;

namespace Trivident.Modules.Core.Services.Preview
{
    /// <summary>
    /// Used to load IPage from posted data.
    /// </summary>
    public interface IPreviewContentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        IWebPage GetPage(string data, string url);
    }
}
