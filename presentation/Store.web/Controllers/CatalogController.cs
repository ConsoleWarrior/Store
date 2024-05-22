using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Store.web.Controllers
{
    public class CatalogController : Controller
    {

		private readonly BookService bookService;

		public CatalogController(BookService bookService)
		{
			this.bookService = bookService;
		}
		public IActionResult Index()
        {
            return View();
        }
		public IActionResult AllBooks()
		{
			var books = bookService.GetAll();
			return View(books);
		}
	}
}
