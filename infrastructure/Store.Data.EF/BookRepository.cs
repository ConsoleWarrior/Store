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

        public async Task AddBookToRepositoryAsync(string isbn, string author, string title, string description, decimal price, string image)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));
            var dto = Book.DtoFactory.Create(isbn, author, title, description, price, image);
            dbContext.Books.Add(dto);
            await dbContext.SaveChangesAsync();
        }

		public async Task<Book[]> GetAllAsync()
		{
			var dbContext = dbContextFactory.Create(typeof(BookRepository));
			var dtos = await dbContext.Books.ToArrayAsync();
			return dtos.Select(Book.Mapper.Map)
					   .ToArray();
		}

        public async Task<int> GetLastAddedBookAsync()
        {
			var dbContext = dbContextFactory.Create(typeof(BookRepository));
            var dto = await dbContext.Books.FromSqlRaw("SELECT top 1 * FROM Books order by id desc").ToArrayAsync();
            return dto[0].Id;
		}
        public async Task RemoveBookFromRepositoryAsync(int bookId)
        {
			var dbContext = dbContextFactory.Create(typeof(BookRepository));

            dbContext.Books.Remove(dbContext.Books
                                     .Single(book => book.Id == bookId));
            await dbContext.SaveChangesAsync();
        }

        public async Task<Book[]> GetAllByIsbnAsync(string isbn)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            if (Book.TryFormatIsbn(isbn, out string formattedIsbn))
            {
                var dtos = await dbContext.Books
                                          .Where(book => book.Isbn == formattedIsbn)
                                          .ToArrayAsync();

                return dtos.Select(Book.Mapper.Map).ToArray();
            }

            return new Book[0];
        }

        public async Task<Book[]> GetAllByTitleOrAuthorAsync(string titleOrAuthor)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var parameter = new SqlParameter("@titleOrAuthor", titleOrAuthor);
            BookDto[] dtos;
            try
            {
                dtos = await dbContext.Books
                                      .FromSqlRaw("SELECT * FROM Books WHERE CONTAINS((Author, Title), @titleOrAuthor)",
                                                  parameter)
                                      .ToArrayAsync();
            }
            catch (Exception ex) { dtos = null; }


            return dtos?.Select(Book.Mapper.Map).ToArray();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var dto = await dbContext.Books.SingleAsync(book => book.Id == id);

            return Book.Mapper.Map(dto);
        }

        public async Task<Book[]> GetAllByIdsAsync(IEnumerable<int> bookIds)
        {
            var dbContext = dbContextFactory.Create(typeof(BookRepository));

            var dtos = await dbContext.Books
                                      .Where(book => bookIds.Contains(book.Id))
                                      .ToArrayAsync();

            return dtos.Select(Book.Mapper.Map)
                       .ToArray();
        }
    }
}
