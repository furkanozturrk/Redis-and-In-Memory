using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }
        public IActionResult Index()
        {
            db.StringSet("name", "Furkan Öztürk");
            db.StringSet("ziyaretci", 100);

            return View();
        }
        public IActionResult Show()
        {
            var value = db.StringGet("name");

            //String dışında bir data tutmak istediğimizde StringSet metotdunu kullanarak Byte yada json formatında data tutabiliriz.
            Byte[] resimByte =   System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.jpg"));
            db.StringSet("resim", resimByte);

            //Gelen datayı 0'dan başlıyarak 3'üncü karaktere kadar aldı.3'de dahil.
            //var value=db.StringGetRange("name", 0, 3);

            //Gelen datanın uzunluğunu alır
            //var value=db.StringLength("name");

            //Datayı 10'ar arttırma
            //db.StringIncrement("ziyaretci", 10);

            //Datayı 1'er azaltma
            //Asekron bir metotda await kullanmak istemiyorsan result kullanabilirsin ama bir değişkene atmak zorundasın.
            //var count = db.StringDecrementAsync("ziyaretci", 1).Result;

            //Asekron bir metotda await kullanmak istemiyorsan ve bir değişkenede atamak istemiyorsan wait() kullanabilirsin.
            //db.StringDecrementAsync("ziyaretci", 1).Wait();

            if (value.HasValue)
            {
                ViewBag.value = value.ToString();
            }

            return View();
        }
    }
}
