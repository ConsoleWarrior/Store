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
        Book GetById(int id);
        Book[] GetAllByIds(IEnumerable<int> bookIds);
		void AddBookToRepository(string isbn, string author, string title, string description, decimal price, string image);
		Book[] GetAll();
        int GetLastAddedBook();
        void RemoveBookFromRepository(int id);
    }
}
