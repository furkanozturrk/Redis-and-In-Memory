using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    //String Data İşlemi
    public class ProductController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions distributedCache = new DistributedCacheEntryOptions();

            distributedCache.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name2", "furkan2", distributedCache);
            await _distributedCache.SetStringAsync("surname", "öztürk", distributedCache);
            return View();
        }
        public IActionResult Show()
        {
            string name = _distributedCache.GetString("name2");
            ViewBag.Name = name;
            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("name2");
            return View();
        }
    }
}
