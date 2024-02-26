using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            #region Memoryde bu keyde veri var mı ?..
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            //{
            //    //data oluşturma
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            ////data oluşturma
            //_memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);
            #endregion

            #region Memoryde bu keyde veri var mı ?..
            //if (!_memoryCache.TryGetValue("zaman", out string zamancache))
            //{
            //    //data oluşturma
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}

            ////data oluşturma
            //_memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);
            #endregion

            #region Memorydeki Cache AbsoluteExpiration ile  süre verme verilen süre bittiğinde ramden ne olursa olsun veri silinir
            //MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            //Memorydeki Cache SlidingExpiration ile  süre verme verilen süre içerisinde ne kadar istek gelirse okadar süre artar istek gelmez ise süre bittiğinde ramden silinir..
            //MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //options.SlidingExpiration = TimeSpan.FromSeconds(10);

            ////data oluşturma
            //_memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);
            #endregion

            #region Best practice Açısında absolute ve sliding'i beraber kullanmaktır.10 saniyede bir istek atılmazsa silinir atılırsa her 1 dakikada bir istek olsa dahi ramdan data silinir..
            //MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //options.SlidingExpiration = TimeSpan.FromSeconds(10);

            //options.Priority ram doldugundan data siliminde CacheItemPriority.Low'dan başlıyarak yukarıya dogru ilerler..
            //MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //options.SlidingExpiration = TimeSpan.FromSeconds(10);

            //options.Priority = CacheItemPriority.Low;

            ////data oluşturma
            //_memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);
            #endregion

            #region RegisterPostEvictionCallback ram'den dataların silinme nedenlerini öğreme
            //MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            //options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

            ////options.SlidingExpiration = TimeSpan.FromSeconds(10);

            //options.Priority = CacheItemPriority.Low;

            //options.RegisterPostEvictionCallback((key, value, reason, state) =>
            //{
            //    _memoryCache.Set<string>("callback", $"{key}-->{value}=> sebep : {reason}");
            //});

            ////data oluşturma
            //_memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);
            #endregion



            Product product = new Product { Id = 1, Name = "Kalem", Price = 100 };

            _memoryCache.Set<Product>("product:1", product);

            return View();
        }

        public IActionResult Show()
        {
            #region oluşturulan datayı çekme 
            //ViewBag.zaman = _memoryCache.Get<string>("zaman");

            //_memoryCache.TryGetValue("zaman", out string zamancache);
            //_memoryCache.TryGetValue("callback", out string callback);

            //ViewBag.zaman = zamancache;
            //ViewBag.callback = callback;
            #endregion

            ViewBag.product = _memoryCache.Get<Product>("product:1");

            return View();

            #region bu keyle ilgili değer alamaya çalışır alamazsa oluşturur
            //_memoryCache.GetOrCreate<string>("zaman", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});

            //ilgili datayı silme
            //_memoryCache.Remove("zaman");
            #endregion
        }
    }
}
