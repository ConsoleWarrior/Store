namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"Белый клык","ISBN0000000001","Джек Лондон",100m,"sdfsf"),
            new Book(2,"Алая чума", "ISBN0000000002", "Джек Лондон",200m,"sdfsf"),
            new Book(3,"Мартин Иден", "ISBN0000000003", "Джек Лондон",300m,"sdfsf"),
            new Book(4,"Джон Ячменное Зерно", "ISBN0000000004","Джек Лондон",200m,"sdfsf"),
            new Book(5,"Черный клык","ISBN0000000005","Неизвестен",1.99m,"sdfsf"),
        };

        public Book[] GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn.Replace(" ", "").Replace("-", "").ToUpper())
                        .ToArray();
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthorPart)
        {
            return books.Where(book => book.Title.Contains(titleOrAuthorPart)||
                                       book.Author.Contains(titleOrAuthorPart))
                        .ToArray();
        }

        public Book GetById(int id)
        {
            return books.Single(book => book.Id == id);
        }
    }
}

