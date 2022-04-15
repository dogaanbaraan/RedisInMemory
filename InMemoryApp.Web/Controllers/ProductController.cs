using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

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
            //Bir key değerinin memoryde olup olmadığının tespit edilmesi
            #region 1.Yol 
            //if (String.IsNullOrEmpty(_memoryCache.Get<string>("zaman"))) //
            //{
            //    _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            //}
            #endregion

            #region 2.Yol
            if (!_memoryCache.TryGetValue("zaman", out string zamanCache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }

            #endregion

            return View();
        }

        public IActionResult Show()
        {
            //_memoryCache.Remove("zaman"); //Memorycache üzerindeki seçmiş olduğumuz datayı cache üzerinde silmek için kullanılır.

            _memoryCache.GetOrCreate<string>("zaman", entry => //Bu methodla beraber bu keye sahip değeri almaya çalışır
                                                               //eğer memoryde bu değer yoksa memory üzerinde bu değeri oluşturur.
            {
                return DateTime.Now.ToString();
            });

            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
