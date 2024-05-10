using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class change7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Author", "Description", "Isbn", "Price", "Title" },
                values: new object[] { "Джек Лондон", "Про пса", "ISBN000000001", 3.33m, "Белый клык" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Author", "Description", "Title" },
                values: new object[] { "Джек Лондон", "Автобиография", "Мартин Иден" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Author", "Description", "Isbn", "Price", "Title" },
                values: new object[] { "Джек Лондон1", "П", "ISBN000000000", 4m, "Белый клык1" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Author", "Description", "Title" },
                values: new object[] { "B. W. Kernighan, D. M. Ritchie", "Known as the bible of C, this classic bestseller introduces the C programming language and illustrates algorithms, data structures, and programming techniques.", "C Programming Language" });
        }
    }
}
