using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ADVANCLOUDMIDDLEWARE.Service
{
       public class AdvanCloudService : IAdvanCloudService
    {
        private readonly HttpClient _httpClient;

        public AdvanCloudService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> ImportProductsAsync(ProductData productData)
        {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://xxxx.keonn.com/advancloud/import/upload")
        {
            Content = new StringContent(JsonConvert.SerializeObject(productData), Encoding.UTF8, "application/json")
        };

        // Assuming the API requires basic authentication
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "username:password");

        return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetDeviceHealthAsync(string token)
        {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://xxxx.keonn.com/advancloud/buspublish/machines/{token}")
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) }
        };

        return await _httpClient.SendAsync(request);
        }
    }
}