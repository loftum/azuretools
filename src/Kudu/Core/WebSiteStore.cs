using System.Collections;
using System.Collections.Generic;
using System.IO;
using Kudu.Extensions;
using Kudu.Logging;

namespace Kudu.Core
{
    public class WebSiteStore : IEnumerable<WebSiteSettings>
    {
        private static readonly ILogger Logger = Log.For<WebSiteStore>();

        private const string Filename = "websites.json";

        private readonly IDictionary<string, WebSiteSettings> _websites;

        public WebSiteStore()
        {
            _websites = Load();
        }

        public WebSiteSettings this[string name]
        {
            get
            {
                var lower = name.ToLowerInvariant();
                WebSiteSettings settings;
                return _websites.TryGetValue(lower, out settings) ? settings : null;
            }
            set
            {
                var lower = name.ToLowerInvariant();
                if (value == null)
                {
                    _websites.Remove(lower);
                }
                else
                {
                    _websites[lower] = value;
                }
                Save();
            }
        }

        public void Save()
        {
            Logger.Debug($"Save {Filename}");
            File.WriteAllText(Filename, _websites.ToJson());
        }

        private static IDictionary<string, WebSiteSettings> Load()
        {
            return File.Exists(Filename)
                ? File.ReadAllText(Filename).FromJsonTo<Dictionary<string, WebSiteSettings>>()
                : new Dictionary<string, WebSiteSettings>();
        }

        public IEnumerator<WebSiteSettings> GetEnumerator()
        {
            return _websites.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}