using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Kudu.Core;
using Newtonsoft.Json;

namespace Kudu.Interactive
{
    public class App
    {
        public static WebSiteStore Store { get; }

        static App()
        {
            Store = new WebSiteStore();
            Settings = Store.FirstOrDefault();
        }

        public static string Site
        {
            get { return Settings?.Name; }
            set
            {
                var current = Store[value];
                if (current == null)
                {
                    current = new WebSiteSettings {Name = value};
                    Store[value] = current;
                }
                Settings = current;
            }
        }

        public static WebSiteSettings Settings { get; private set; }

        public static IEnumerable<string> Sites => Store.Select(s => s.Name);

        public static object WebJobs
        {
            get
            {
                var request = WebRequest.CreateHttp($"{Settings.GetApiBase()}/webjobs");
                request.Accept = "application/json";
                var bytes = Encoding.UTF8.GetBytes($"{Settings.GetUserName()}:{Settings.Password}");
                var auth = Convert.ToBase64String(bytes);
                request.Headers["Authorization"] = $"Basic {auth}";

                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                        {
                            throw new ApplicationException("Stream is null. Wat.");
                        }
                        using (var reader = new StreamReader(stream))
                        {
                            var json = reader.ReadToEnd();
                            return JsonConvert.DeserializeObject(json);
                        }
                    }
                }
            }
        }
    }
}