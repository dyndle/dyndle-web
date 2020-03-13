namespace Dyndle.Modules.Management.Models
{
    public class CacheItem
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public string Region { get; set; }
    }
}
