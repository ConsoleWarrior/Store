using Microsoft.AspNetCore.Mvc;

namespace Store.YandexCassa.Areas.YandexCassa.Controllers
{
	[Area("YandexCassa")]
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CallBack()
		{
			return View();
		}
	}
}
