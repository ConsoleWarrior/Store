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

        public async Task<IActionResult> Index(int id)
        {
            var model = await bookService.GetByIdAsync(id);
            return View(model);
        }

        public IActionResult InputNewBook()
        {
            return View();
        }

        public async Task<IActionResult> AddBookResult(string isbn,
                                         string author,
                                         string title,
                                         string description,
                                         string price,
                                         string image)
        {
            int bookId;
            try
            {
                bookId = await bookService.AddNewBookAsync(isbn, author, title, description, Convert.ToDecimal(price), image);
            }
            catch { return View("Error", new ErrorViewModel()); }
            return View(await bookService.GetByIdAsync(bookId));
        }

        public async Task<IActionResult> RemoveBookFromDB(int bookId)
        {
            await bookService.RemoveBookAsync(bookId);
            return RedirectToAction("Index", "Home");
        }
    }
}
