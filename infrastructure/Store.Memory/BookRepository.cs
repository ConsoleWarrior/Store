namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"Белый клык"),
            new Book(2,"Алая чума"),
            new Book(3,"Мартин Иден"),
            new Book(4,"Джон Ячменное Зерно"),
            new Book(5,"Черный клык"),
        };
        public Book[] GetAllByTitle(string titlePart)
        {
            return books.Where(book => book.Title.Contains(titlePart)).ToArray();
        }
    }
}

