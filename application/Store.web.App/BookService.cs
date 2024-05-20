using Store.web.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class BookService
    {
        private readonly IBookRepository bookRepository;
        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

		public IReadOnlyCollection<BookModel> GetAllByQuery(string query)
        {
            var books = Book.IsIsbn(query)
                ? bookRepository.GetAllByIsbn(query)
                : (query != null) ? bookRepository.GetAllByTitleOrAuthor(query)
                : null;
            return books?.Select(Map).ToArray();
            //if (query == null) return new Book[0];
            //else if(Book.IsIsbn(query)) return bookRepository.GetAllByIsbn(query);
            //else return bookRepository.GetAllByTitleOrAuthor(query);
        }

		public BookModel GetById(int id)
		{
			var book = bookRepository.GetById(id);
            return Map(book);
		}

		private BookModel Map(Book book)
		{
            return new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Isbn = book.Isbn,
                Description = book.Description,
                Price = book.Price,
                Image = book.Image
            };
		}
		public int AddNewBook(string isbn, string author, string title, string description, decimal price, string image)
		{
            bookRepository.AddBookToRepository(isbn, author, title, description, price, image);
            Thread.Sleep(1000);
            var books = bookRepository.GetAllByTitleOrAuthor(title);
            if (books.Length == 0) throw new InvalidOperationException("Книга с таким названием не найдена, ошибка при добавлении в базу данных");
			return books[0].Id;
		}
	}
}
