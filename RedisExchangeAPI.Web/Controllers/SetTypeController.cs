using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        private string listKey = "hashnames";
        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }
        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    nameList.Add(x);
                });
            }
            return View(nameList);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            //Redis tarafında sliding expiration olmadıgından böyle yaparak sliding expiration  özelliğini kazandırabiliriz.
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            db.SetAdd(listKey, name);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteItem(string name)
        {
            await db.SetRemoveAsync(listKey, name);

            return RedirectToAction("Index");
        }
    }
}
