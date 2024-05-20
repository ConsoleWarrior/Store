using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.EF
{
    class BookRepository : IBookRepository
    {
        private readonly DbContextFactory dbContextFactory;

        public BookRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var dtos = dbContext.Books
                                      .Where(book => bookIds.Contains(book.Id))
                                      .ToArray();

            return dtos.Select(Book.Mapper.Map)
                       .ToArray();
        }

        public Book[] GetAllByIsbn(string isbn)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            if (Book.TryFormatIsbn(isbn, out string formattedIsbn))
            {
                var dtos = dbContext.Books
                                          .Where(book => book.Isbn == formattedIsbn)
                                          .ToArray();

                return dtos.Select(Book.Mapper.Map)
                           .ToArray();
            }

            return new Book[0];
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var parameter = new SqlParameter("@titleOrAuthor", titleOrAuthor);
            var dtos = dbContext.Books
                                      .FromSqlRaw("SELECT * FROM Books WHERE CONTAINS((Author, Title), @titleOrAuthor)",
                                                  parameter)
                                      .ToArray();

            return dtos.Select(Book.Mapper.Map)
                       .ToArray();
        }

        public Book GetById(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var dto = dbContext.Books
                                     .Single(book => book.Id == id);

            return Book.Mapper.Map(dto);
        }

        public void AddBookToRepository(string isbn, string author, string title, string description, decimal price, string image)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));
            var dto = Book.DtoFactory.Create(isbn, author, title, description, price, image);
            dbContext.Books.Add(dto);
            dbContext.SaveChanges();
            //return Book.Mapper.Map(dto);
        }
    }
}
