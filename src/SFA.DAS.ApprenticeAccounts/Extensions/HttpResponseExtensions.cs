using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SFA.DAS.ApprenticeAccounts.Extensions
{
    public static class HttpResponseExtensions
    {
        public static Task WriteJsonAsync(this HttpResponse httpResponse, object body)
        {
            httpResponse.ContentType = "application/json";

            return httpResponse.WriteAsync(JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                Formatting = Formatting.Indented
            }));
        }
    }
}
