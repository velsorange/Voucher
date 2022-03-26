using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace IntegrationTests;

public static class StringContentExtensions
{
    public static StringContent MakeStringContent(this object o)
        => new StringContent(JsonConvert.SerializeObject(o, Formatting.None, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore
        }), Encoding.UTF8, "application/json");

}