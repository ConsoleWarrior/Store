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

		public async Task<IActionResult> AllBooks()
		{
			var books = await bookService.GetAllAsync();
			return View(books);
		}
	}
}
