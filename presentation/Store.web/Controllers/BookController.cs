using Microsoft.AspNetCore.Mvc;

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
										 decimal price,
										 string image)
        {
            int bookId = bookService.AddNewBook(isbn, author, title, description, price, image);
            return View(bookService.GetById(bookId));
        }
    }
}
