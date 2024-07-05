
namespace Store
{
    public interface IBookRepository
    {
		Task AddBookToRepositoryAsync(string isbn, string author, string title, string description, decimal price, string image);
		Task<Book[]> GetAllAsync();
        Task<int> GetLastAddedBookAsync();
        Task RemoveBookFromRepositoryAsync(int id);
        Task<Book[]> GetAllByIsbnAsync(string query);
        Task<Book[]> GetAllByTitleOrAuthorAsync(string query);
        Task<Book> GetByIdAsync(int id);
        Task<Book[]> GetAllByIdsAsync(IEnumerable<int> bookIds);
    }
}
