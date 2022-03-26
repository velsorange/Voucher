using System.Net.Http;
using System.Threading.Tasks;

namespace IntegrationTests;

internal static class HttpClientExtensions
{
    public static async Task<T> PostAsync<T>(this HttpClient httpClient, string requestUri, HttpContent content)
        => await ProcessRequest<T>(await httpClient.PostAsync(requestUri, content));

    public static async Task<T> PostAsync<T>(this HttpClient httpClient, string requestUri)
        => await ProcessRequest<T>(await httpClient.PostAsync(requestUri, null));

    private static async Task<T> ProcessRequest<T>(HttpResponseMessage httpResponseMessage)
    {
        httpResponseMessage.EnsureSuccessStatusCode();
        var result = await httpResponseMessage.GetContentAsAsync<T>();

        return result;
    }
}