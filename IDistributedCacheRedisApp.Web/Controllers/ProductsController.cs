using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    //Class Data İşlemi
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            #region String Class işlemi
            //DistributedCacheEntryOptions distributedCache = new DistributedCacheEntryOptions();

            //distributedCache.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            //Product product = new Product { Id = 2, Name = "kalem2", Price = 200 };

            //string jsonProduct = JsonSerializer.Serialize(product);

            //await _distributedCache.SetStringAsync("Product:2", jsonProduct, distributedCache);
            #endregion

            #region Byte Dönüştürüp işlemi gerçekleştirme

            DistributedCacheEntryOptions distributedCache = new DistributedCacheEntryOptions();

            distributedCache.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            Product product = new Product { Id = 1, Name = "kalem1", Price = 100 };

            string jsonProduct = JsonSerializer.Serialize(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            await _distributedCache.SetAsync("byteproduct:2", byteProduct, distributedCache);

            #endregion

            return View();
        }
        public async Task<IActionResult> Show()
        {
            #region String Class işlemi
            //string product = await _distributedCache.GetStringAsync("Product:1");

            //Product prod = JsonSerializer.Deserialize<Product>(product);
            //ViewBag.product = prod;

            #endregion

            #region Byte Dönüştürüp işlemi gerçekleştirme

            Byte[] byteProduct =  _distributedCache.Get("byteproduct:1");

            string jsonProduct=Encoding.UTF8.GetString(byteProduct);

            Product prod = JsonSerializer.Deserialize<Product>(jsonProduct);
            ViewBag.product = prod;
            #endregion

            return View();
        }
        public async Task<IActionResult> Remove()
        {
            await _distributedCache.GetStringAsync("Product:1");
            return View();
        }
    }
}
