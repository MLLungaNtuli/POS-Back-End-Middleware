using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ADVANCLOUDMIDDLEWARE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvanCloudController : ControllerBase
    {
        private readonly IAdvanCloudService _advanCloudService;

        public AdvanCloudController(IAdvanCloudService advanCloudService)
        {
            _advanCloudService = advanCloudService;
        }

        [HttpPost("import-products")]
        public async Task<IActionResult> ImportProducts([FromBody] ProductDTO productData)
        {
             var request = new HttpRequestMessage(HttpMethod.Post, "https://xxxx.keonn.com/advancloud/import/upload")
        {
            Content = new StringContent(JsonConvert.SerializeObject(productData), Encoding.UTF8, "application/json")
        };

        // Assuming the API requires basic authentication
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "username:password");

        return await _httpClient.SendAsync(request);
        }

        [HttpGet("device-health/{token}")]
        public async Task<IActionResult> GetDeviceHealth(string token)
        {
        var response = await _advanCloudService.GetDeviceHealthAsync(token);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
        return BadRequest("Error fetching device health.");
        }
    }

}