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

		public Book[] GetAllByIds(IEnumerable<int> bookIds)
		{
			return ResultGetAllByIds;
		}

		public Book[] GetAllByIsbn(string isbn)
        {
            return ResultGetAllByIsbn;
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthorPart)
        {
            return ResultGetAllByTitleOrAuthor;
        }

        public Book GetById(int id)
        {
            return ResultGetById;
        }
    }
}
