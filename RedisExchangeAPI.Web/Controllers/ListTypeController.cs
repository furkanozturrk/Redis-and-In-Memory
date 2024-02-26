using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string listKey = "names";
        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            List<string> namesList = new List<string>();

            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x);
                });
            }
            return View(namesList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            //Listin başına ekleme
            //db.ListLeftPush(listKey, name);

            //Listin sonuna ekleme
            db.ListRightPush(listKey, name);

            return RedirectToAction("Index");
        }
         
        public IActionResult DeleteItem(string name)
        {
            db.ListRemoveAsync(listKey, name);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteFirstItem()
        {
            //Listenin İlk değerinden siler
            db.ListLeftPop(listKey);

            //Listenin son değerinden siler
            db.ListRightPop(listKey);
            return RedirectToAction("Index");
        }
    }
}
