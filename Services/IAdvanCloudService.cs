using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ADVANCLOUDMIDDLEWARE.Service
{
    public interface IAdvanCloudService
    {
        Task<HttpResponseMessage> ImportProductsAsync(ProductDTO productData);
        Task<HttpResponseMessage> GetDeviceHealthAsync(string token);
    }
}