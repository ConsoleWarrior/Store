namespace Store;

public class Book
{
    public int Id { get;}
    public string Title { get;}
    public int Isbn { get; }
    public string Author { get; }

    public Book(int id, string title, int isbn, string author)
    {
        Id = id;
        Title = title;
        Isbn = isbn;
        Author = author;
    }
}
