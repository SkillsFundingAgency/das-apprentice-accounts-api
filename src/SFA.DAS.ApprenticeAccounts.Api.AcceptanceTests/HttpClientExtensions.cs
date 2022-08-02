using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeAccounts.Api.AcceptanceTests
{
    public static class HttpClientExtensions
    {
        public static async Task<(HttpResponseMessage, T)> GetValueAsync<T>(this HttpClient client, string url)
        {
            using var response = await client.GetAsync(url);
            return await ProcessResponse<T>(response);
        }

        public static async Task<HttpResponseMessage> PostValueAsync<T>(this HttpClient client, string url, T data)
        {
            return await client.PostAsync(url, data.GetStringContent());
        }

        public static async Task<HttpResponseMessage> PutValueAsync<T>(this HttpClient client, string url, T data)
        {
            return await client.PutAsync(url, data.GetStringContent());
        }

        public static async Task<HttpResponseMessage> PatchValueAsync<T>(this HttpClient client, string url, T data)
        {
            return await client.PatchAsync(url, data.GetStringContent());
        }

        private static async Task<(HttpResponseMessage, T)> ProcessResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent)
                return (response, default);

            var content = await response.Content.ReadAsStringAsync();
            var responseValue = JsonSerializer.Deserialize<T>(content);

            return (response, responseValue);
        }

        public static StringContent GetStringContent(this object obj)
            => new StringContent(JsonSerializer.Serialize(obj), Encoding.Default, "application/json");
    }
}
