using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationTests;

public static class HttpResponseMessageExtensions
{
    public static async Task<T> GetContentAsAsync<T>(this HttpResponseMessage httpResponseMessage)
    {
        return JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
    }
}