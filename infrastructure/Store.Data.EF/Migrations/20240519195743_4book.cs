using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class _4book : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "Description", "Image", "Isbn", "Price", "Title" },
                values: new object[] { 4, "Джек Лондон", "Автобиография - V2", "images/back.jpeg", "ISBN013110163", 9.99m, "Джон Ячменное зерно" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
