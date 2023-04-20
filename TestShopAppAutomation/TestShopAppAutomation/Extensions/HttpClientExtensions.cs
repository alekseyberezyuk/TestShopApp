using System;
using System.Text;
using Newtonsoft.Json;

namespace TestShopAppAutomation.Extensions
{
    internal static class HttpClientExtensions
    {
        internal static async Task<HttpResponseMessage> Post<T>(this HttpClient client, string url, T request)
        {
            string jsonRequestBody = JsonConvert.SerializeObject(request);
            StringContent requestContent = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, requestContent);
            client.Dispose();

            return response;
        }

        internal static async Task<HttpResponseMessage> Get<T>(this HttpClient client, string url)
        {
            HttpResponseMessage response = await client.GetAsync(url);
            client.Dispose();

            return response;
        }
    }
}
