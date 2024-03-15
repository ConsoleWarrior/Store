using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetAllByIsbn()
        {
            const int idOfIsbnSearch = 1;
            const int idOfAuthorSearch = 2;

            var bookRepository = new StubBookRepository();
            bookRepository.ResultGetAllByIsbn = new[]
            {
                new Book(idOfIsbnSearch, "", "", "", 100, "sdfsf")
            };
            bookRepository.ResultGetAllByTitleOrAuthor = new[]
{
                new Book(idOfAuthorSearch, "", "", "", 100, "sdfsf")
            };

            var bookService = new BookService(bookRepository);

            var books = bookService.GetAllByQuery("ISBN1002003004");

            Assert.Collection(books, book => Assert.Equal(idOfIsbnSearch, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithAuthorOrTitle_CallGetAllByTitleOrAuthor()
        {
            const int idOfIsbnSearch = 1;
            const int idOfAuthorSearch = 2;

            var bookRepository = new StubBookRepository();
            bookRepository.ResultGetAllByIsbn = new[]
            {
                new Book(idOfIsbnSearch, "", "", "", 100, "sdfsf")
            };
            bookRepository.ResultGetAllByTitleOrAuthor = new[]
{
                new Book(idOfAuthorSearch, "", "", "", 100, "sdfsf")
            };

            var bookService = new BookService(bookRepository);

            var books = bookService.GetAllByQuery("Test");

            Assert.Collection(books, book => Assert.Equal(idOfAuthorSearch, book.Id));
        }
    }
}
