using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Dyndle.Modules.Core.Attributes.Filter
{
    /// <summary>
    /// Filter used to override the default Mvc stream writer
    /// Manipulating the baseUrl's for scripts and styling tags
    /// </summary>
    public class AbsoluteUrlFilter : MemoryStream
    {
        private readonly StreamWriter _shrink;
        private readonly Regex _linkRegex;
        private readonly Regex _scriptRegex;
        private readonly Regex _baseHrefRegex;
        private readonly Regex _headRegex;
        private readonly Regex _htmlRegex;
        private readonly Uri _baseUri;
        private const string HrefUrlPattern = "(?<=href=[\"|'])(.*?)(?=[\"|'])";
        private const string SrcUrlPattern = "(?<=src=[\"|'])(.*?)(?=[\"|'])";

        /// <summary>
        /// create a instance of AbsoluteUrlFilter
        /// </summary>
        /// <param name="stream">The response stream.</param>
        /// <param name="baseUri">The base URI.</param>
        public AbsoluteUrlFilter(Stream stream, Uri baseUri)
        {
            _shrink = new StreamWriter(stream);
            _linkRegex = new Regex("<link\\s[^>]*href=(?!\"http)(.*).css[^>]*>", RegexOptions.Compiled);
            _scriptRegex = new Regex("<script\\s[^>]*src=(?!\"http)(.*).js[^>]*>", RegexOptions.Compiled);
            _baseHrefRegex = new Regex("<base[^>]+\\bhref\\b[^>]*>", RegexOptions.Compiled);
            _headRegex = new Regex("<head\\b[^>]*>", RegexOptions.Compiled);
            _htmlRegex = new Regex("<html\\b[^>]*>", RegexOptions.Compiled);
            _baseUri = baseUri;
        }
        /// <summary>
        /// Override the Close method of the base.
        /// </summary>
        public override void Close()
        {
            byte[] allBytes = this.ToArray();
            string payload = Encoding.UTF8.GetString(allBytes);
            if (_baseHrefRegex.IsMatch(payload))
            {
                payload = _baseHrefRegex.Replace(payload, $"<base href=\"{_baseUri}\" />");
            }
            else if (_headRegex.IsMatch(payload))
            {
                payload = _headRegex.Replace(payload, $"$&\r\n<base href=\"{_baseUri}\" />");
            }
            else if (_htmlRegex.IsMatch(payload))
            {
                payload = _htmlRegex.Replace(payload, $"$&\r\n<base href=\"{_baseUri}\" />");
            }
            else
            {
                payload += $"<base href=\"{_baseUri}\" />\r\n";
            }

            //payload = ServeAbsoluteUrls(payload, _linkRegex.Matches(payload), HrefUrlPattern);
            //payload = ServeAbsoluteUrls(payload, _scriptRegex.Matches(payload), SrcUrlPattern);

            _shrink.Write(payload);
            _shrink.Flush();
            _shrink.Close();
            base.Close();
        }

        private string ServeAbsoluteUrls(string payload, MatchCollection collection, string urlPattern)
        {
            foreach (Match match in collection)
            {
                var url = Regex.Match(match.Value, urlPattern, RegexOptions.Compiled).Value;
                url = url.StartsWith("~") ? url.Replace("~", string.Empty) : url;
                if (!url.StartsWith(_baseUri.AbsolutePath))
                {
                    url = string.Concat(_baseUri.AbsolutePath, url);
                }
                var baseUri = new Uri(_baseUri, url);
                var replaced = Regex.Replace(match.Value, urlPattern, baseUri.ToString());

                payload = payload.Replace(match.Value, replaced);
            }

            return payload;
        }
    }

}