using Microsoft.AspNetCore.Mvc;

namespace Store.web.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		public IActionResult AllBooks()
		{
			return View();
		}
	}
}
