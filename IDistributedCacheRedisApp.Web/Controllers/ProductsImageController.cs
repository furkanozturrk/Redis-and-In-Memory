using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsImageController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductsImageController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions distributedCache = new DistributedCacheEntryOptions();

            distributedCache.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            Product product = new Product { Id = 2, Name = "kalem2", Price = 200 };

            string jsonProduct = JsonSerializer.Serialize(product);

            await _distributedCache.SetStringAsync("Product:2", jsonProduct, distributedCache);

            return View();
        }
        public async Task<IActionResult> Show()
        {
            string product = await _distributedCache.GetStringAsync("Product:1");

            Product prod = JsonSerializer.Deserialize<Product>(product);
            ViewBag.product = prod;

            return View();
        }
        public async Task<IActionResult> Remove()
        {
            await _distributedCache.GetStringAsync("Product:1");
            return View();
        }
        public async Task<IActionResult> ImageUrl()
        {
            byte[] resimByte = _distributedCache.Get("resim");
            return File(resimByte,"image/jpeg");
        }
        public async Task<IActionResult> ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim", imageByte);


            return View();
        }
    }
}
