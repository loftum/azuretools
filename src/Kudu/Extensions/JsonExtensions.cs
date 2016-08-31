using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Kudu.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object item, bool indented = false)
        {
            var settings = new JsonSerializerSettings();
            var formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(item, formatting, settings);
        }

        public static string ToJsonCamelCase(this object item, bool indented = false)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(item, formatting, settings);
        }

        public static object ToJsonObject(this string json)
        {
            return JsonConvert.DeserializeObject(json);
        }

        public static T FromJsonTo<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}