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


        public Book[] GetAllByIsbn(string isbn)
        {
            return ResultGetAllByIsbn;
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthorPart)
        {
            return ResultGetAllByTitleOrAuthor;
        }
    }
}
