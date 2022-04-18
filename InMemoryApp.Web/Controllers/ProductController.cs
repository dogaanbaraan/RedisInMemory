using InMemoryApp.Web.Models;
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
            //if (!_memoryCache.TryGetValue("zaman", out string zamanCache))
            //{
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(10); /*verilen süre geçtikten sonra data cache üzerinden silinir.*/
                /*options.SlidingExpiration = TimeSpan.FromSeconds(10);*/    //verilen süre tam olarak geçmeden o süre içerisinde
                                                                         //veri istenirse verilen süre sürekli yenilenir.
                options.Priority = CacheItemPriority.High;               //cachemiz dolduğunda silinecek olan datayı seçebilmek için datalara önem derecesi aktarılır.
                                                                         //bu sayede önemli data dururken önemsiz data silinir.

            options.RegisterPostEvictionCallback((key, value, reason, state) => //memoryden bir data silindiği zaman hangi sebeple silindiğini bildirmek için kullanılır.
            {
                _memoryCache.Set("callback", $"{key} -> , {value} => sebep:{reason}");
            });
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            Product product = new Product { Id = 1, Name = "Kalem", Price = 200 };
            _memoryCache.Set<Product>("product:1", product);
            return View();
            //}

            #endregion

        }

        public IActionResult Show()
        {
            //_memoryCache.Remove("zaman"); //Memorycache üzerindeki seçmiş olduğumuz datayı cache üzerinde silmek için kullanılır.

            //_memoryCache.GetOrCreate<string>("zaman", entry => //Bu methodla beraber bu keye sahip değeri almaya çalışır
            //                                                   //eğer memoryde bu değer yoksa memory üzerinde bu değeri oluşturur.
            //{
            //    return DateTime.Now.ToString();
            //});

            _memoryCache.TryGetValue<string>("zaman", out string zamanCache);
            _memoryCache.TryGetValue<string>("callback", out string callCache);
            _memoryCache.TryGetValue<Product>("product:1", out Product productCache);

            ViewBag.zaman = zamanCache;
            ViewBag.callCache = callCache;
            ViewBag.product = productCache;
            return View();
        }
    }
}
