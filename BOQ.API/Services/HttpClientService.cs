using BOQ.API.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace BOQ.API.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;
        private readonly string baseUri;
        public HttpClientService(HttpClient httpClient, IOptions<DemoConferenceAPISettings> demoConferenceAPISettings)
        {
            this.httpClient = httpClient;
            baseUri = demoConferenceAPISettings.Value.BaseUri;
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var response = await httpClient.GetStringAsync($"{baseUri}{uri}");
            return JsonConvert.DeserializeObject<T>(response);
        }

        public async Task<string> GetAsync(string uri)
        {
            return await httpClient.GetStringAsync($"{baseUri}{uri}");
        }
    }
}
