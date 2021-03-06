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
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            return View();
        }

        public IActionResult Show()
        {
            ViewBag.zaman =  _memoryCache.Get<string>("zaman");
            return View();
        }
    }
}
