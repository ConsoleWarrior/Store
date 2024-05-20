using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Store.Data.EF
{
    public class StoreDbContext : DbContext
    {
        public DbSet<BookDto> Books { get; set; }

        public DbSet<OrderDto> Orders { get; set; }

        public DbSet<OrderItemDto> OrderItems { get; set; }

        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildBooks(modelBuilder);
            BuildOrders(modelBuilder);
            BuildOrderItems(modelBuilder);
        }
        private static void BuildBooks(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookDto>(action =>
            {
				action.Property(dto => dto.Isbn)
                      .HasMaxLength(17)
                      .IsRequired();

                action.Property(dto => dto.Title)
                      .IsRequired();

                action.Property(dto => dto.Price)
                      .HasColumnType("money");

                action.HasData(
                    new BookDto
                    {
                        Id = 1,
                        Isbn = "ISBN0000000001",
                        Author = "Джек Лондон",
                        Title = "Белый клык",
                        Description = "Про пса",
                        Price = 3.33m,
                        Image = "images/corgi.jpeg"
                    },
                    new BookDto
                    {
                        Id = 2,
                        Isbn = "ISBN0201485672",
                        Author = "M. Fowler",
                        Title = "Refactoring",
                        Description = "As the application of object technology--particularly the Java programming language--has become commonplace, a new problem has emerged to confront the software development community.",
                        Price = 12.45m,
						Image = "images/book.jpeg"
					},
                    new BookDto
                    {
                        Id = 3,
                        Isbn = "ISBN0131101633",
                        Author = "Джек Лондон",
                        Title = "Мартин Иден",
                        Description = "Автобиография",
                        Price = 14.98m,
						Image = "images/10.png"
                    },                 
                    new BookDto
                    {
                        Id = 4,
                        Isbn = "ISBN013110163",
                        Author = "Джек Лондон",
                        Title = "Джон Ячменное зерно",
                        Description = "Про алкаша",
                        Price = 9.99m,
                        Image = "images/back.jpeg"
                    }

                );
            });
        }

        private static readonly ValueComparer DictionaryComparer =
            new ValueComparer<Dictionary<string, string>>(
                (dictionary1, dictionary2) => dictionary1.SequenceEqual(dictionary2),
                dictionary => dictionary.Aggregate(
                    0,
                    (a, p) => HashCode.Combine(HashCode.Combine(a, p.Key.GetHashCode()), p.Value.GetHashCode())
                )
            );
        private static void BuildOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDto>(action =>
            {
                action.Property(dto => dto.CellPhone)
                      .HasMaxLength(20)
                      .IsRequired(false);

                action.Property(dto => dto.DeliveryUniqueCode)
                      .IsRequired(false)
                      .HasMaxLength(40);
                
                action.Property(dto => dto.DeliveryDescription)
                      .IsRequired(false);
                
                action.Property(dto => dto.PaymentDescription)
                      .IsRequired(false);

                action.Property(dto => dto.DeliveryPrice)
                      .HasColumnType("money");

                action.Property(dto => dto.PaymentServiceName)
                      .IsRequired(false)
                      .HasMaxLength(40);

                action.Property(dto => dto.DeliveryParameters)
                      .IsRequired(false)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);

                action.Property(dto => dto.PaymentParameters)
                      .IsRequired(false)
                      .HasConversion(
                          value => JsonConvert.SerializeObject(value),
                          value => JsonConvert.DeserializeObject<Dictionary<string, string>>(value))
                      .Metadata.SetValueComparer(DictionaryComparer);
            });
        }

        private void BuildOrderItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItemDto>(action =>
            {
                action.Property(dto => dto.Price)
                      .HasColumnType("money");

                action.HasOne(dto => dto.Order)
                      .WithMany(dto => dto.Items)
                      .IsRequired();
            });
        }
    }
}
