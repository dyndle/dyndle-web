using System;
using System.Configuration;

namespace Trivident.Modules.ImageEnhancement.Models
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
                    _localPath = ConfigurationManager.AppSettings[ImageEnhancementConstants.Settings.LocalPath];
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
                    _cacheSeconds = Convert.ToInt32(ConfigurationManager.AppSettings[ImageEnhancementConstants.Settings.CacheSeconds]);
                }
                return _cacheSeconds;
            }
        }

    }
}
