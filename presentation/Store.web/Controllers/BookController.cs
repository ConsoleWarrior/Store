using Microsoft.AspNetCore.Mvc;
using Store.web.Models;

namespace Store.web.Controllers
{
	public class BookController : Controller
	{
		private readonly BookService bookService;

		public BookController(BookService bookService)
		{
			this.bookService = bookService;
		}
		public IActionResult Index(int id)
		{
			var model = bookService.GetById(id);
			return View(model);
		}
		public IActionResult InputNewBook()
		{

			return View();
		}
		public IActionResult AddBookResult(string isbn,
										 string author,
										 string title,
										 string description,
										 string price,
										 string image)
		{
			int bookId;
			try
			{
				bookId = bookService.AddNewBook(isbn, author, title, description, Convert.ToDecimal(price), image);
			}
			catch (Exception ex) { return View("Error", new ErrorViewModel()); }
			return View(bookService.GetById(bookId));
		}
		public IActionResult RemoveBookFromDB(int bookId)
		{
			bookService.Remove(bookId);
			return RedirectToAction("Index","Home");
		}

    }
}
