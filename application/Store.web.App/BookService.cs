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

        public async Task<IReadOnlyCollection<BookModel>> GetAllByQueryAsync(string query)
        {
            var books = Book.IsIsbn(query)
                ? await bookRepository.GetAllByIsbnAsync(query)
                : (query != null) ? await bookRepository.GetAllByTitleOrAuthorAsync(query)
                : null;
            return books?.Select(Map).ToArray();
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
        public async Task<int> AddNewBookAsync(string isbn, string author, string title, string description, decimal price, string image)
        {
            await bookRepository.AddBookToRepositoryAsync(isbn, author, title, description, price, image);
            return await bookRepository.GetLastAddedBookAsync();
        }
        public async Task RemoveBookAsync(int id)
        {
            await bookRepository.RemoveBookFromRepositoryAsync(id);
        }
        public async Task<IReadOnlyCollection<BookModel>> GetAllAsync()
        {
            var books = await bookRepository.GetAllAsync();

            return books?.Select(Map).ToArray();
        }

        public async Task<BookModel> GetByIdAsync(int id)
        {
            var book = await bookRepository.GetByIdAsync(id);
            return Map(book);
        }
    }
}
