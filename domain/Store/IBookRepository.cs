using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IBookRepository
    {
        Book[] GetAllByTitleOrAuthor(string titleOrAuthorPart);
        Book[] GetAllByIsbn(string isbn);
    }
}
