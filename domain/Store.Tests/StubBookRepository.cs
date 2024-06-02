using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests
{
    internal class StubBookRepository : IBookRepository
    {
        public Book[] ResultGetAllByIsbn { get; set; }
        public Book[] ResultGetAllByTitleOrAuthor { get; set; }
        public Book[] ResultGetAllByIds { get; set; }
        public Book ResultGetById {  get; set; }

		public void AddBookToRepositoryAsync(string isbn, string author, string title, string description, decimal price, string image)
		{
			throw new NotImplementedException();
		}

        public Book[] GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Book[]> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByIds(IEnumerable<int> bookIds)
		{
			return ResultGetAllByIds;
		}

        public Task<Book[]> GetAllByIdsAsync(IEnumerable<int> bookIds)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByIsbn(string isbn)
        {
            return ResultGetAllByIsbn;
        }

        public Task<Book[]> GetAllByIsbnAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthorPart)
        {
            return ResultGetAllByTitleOrAuthor;
        }

        public Task<Book[]> GetAllByTitleOrAuthorAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Book GetById(int id)
        {
            return ResultGetById;
        }

        public Task<Book> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public int GetLastAddedBook()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetLastAddedBookAsync()
        {
            throw new NotImplementedException();
        }

        public void RemoveBookFromRepository(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBookFromRepositoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task IBookRepository.AddBookToRepositoryAsync(string isbn, string author, string title, string description, decimal price, string image)
        {
            throw new NotImplementedException();
        }
    }
}
