namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly Book[] books = new[]
        {
            new Book(1,"Белый клык","ISBN0000000001","Джек Лондон","повесть",3.25m),
            new Book(2,"Алая чума", "ISBN0000000002", "Джек Лондон","рассказ", 1.5m),
            new Book(3,"Мартин Иден", "ISBN0000000003", "Джек Лондон","роман", 1.5m),
            new Book(4,"Джон Ячменное Зерно", "ISBN0000000004","Джек Лондон","повесть", 1.5m),
            new Book(5,"Черный клык","ISBN0000000005","Неизвестен","sdfsf",6.99m),
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
		public Book[] GetAllByIds(IEnumerable<int> bookIds)
		{
			var foundBooks = from book in books
                             join bookId in bookIds on book.Id equals bookId
                             select book;
            return foundBooks.ToArray();
		}
	}
}

