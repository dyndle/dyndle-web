using System;
using System.Configuration;

namespace Dyndle.Modules.ImageEnhancement.Models
{
    public class WebConfiguration : IConfiguration
    {
        private string _localPath;
        private string _backgroundColor;
        private int _cacheSeconds = -1;

        public string LocalPath
        {
            get
            {
                if (_localPath == null)
                {
                    _localPath = ConfigurationManager.AppSettings[ImageEnhancementConstants.Settings.LocalPath] ?? "/EnhancedImages";
                }
                return _localPath;
            }
        }
        public string BackgroundColor
        {
            get
            {
                if (_backgroundColor == null)
                {
                    _backgroundColor = ConfigurationManager.AppSettings[ImageEnhancementConstants.Settings.BackgroundColor];
                }
                return _backgroundColor;
            }
        }

        public int CacheSeconds
        {
            get
            {
                if (_cacheSeconds == -1)
                {
                    var seconds = ConfigurationManager.AppSettings[ImageEnhancementConstants.Settings.CacheSeconds];
                    _cacheSeconds = Convert.ToInt32(seconds ?? "300");
                }
                return _cacheSeconds;
            }
        }

    }
}
