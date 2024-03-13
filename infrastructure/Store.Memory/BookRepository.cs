namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"Белый клык",100,"Джек Лондон"),
            new Book(2,"Алая чума", 101, "Джек Лондон"),
            new Book(3,"Мартин Иден", 103, "Джек Лондон"),
            new Book(4,"Джон Ячменное Зерно", 104,"Джек Лондон"),
            new Book(5,"Черный клык",1000,"Неизвестен"),
        };

        public Book[] GetAllByIsbn(string isbn)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByTitle(string titlePart)
        {
            return books.Where(book => book.Title.Contains(titlePart)).ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthorPart)
        {
            throw new NotImplementedException();
        }
    }
}

