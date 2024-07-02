using Microsoft.AspNetCore.Mvc;

namespace Store.web.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
