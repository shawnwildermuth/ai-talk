using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SnackBarPOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IconEmoji = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AmountTendered = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Change = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "IconEmoji", "IsActive", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Frituur snacks", "🍟", true, "Snacks", 1 },
                    { 2, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Broodjes en sandwiches", "🥪", true, "Broodjes", 2 },
                    { 3, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Koffie, thee en chocolademelk", "☕", true, "Warme Dranken", 3 },
                    { 4, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Koude dranken", "🥤", true, "Frisdranken", 4 },
                    { 5, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Snoep, koekjes en gebak", "🍪", true, "Snoep & Koek", 5 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "ImageUrl", "IsActive", "IsAvailable", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Kleine portie patat", null, true, true, "Patat Klein", 2.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Grote portie patat", null, true, true, "Patat Groot", 3.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Rundvleeskroket", null, true, true, "Kroket", 2.00m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Klassieke frikandel", null, true, true, "Frikandel", 1.80m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "6 stuks bitterballen", null, true, true, "Bitterballen (6 st)", 4.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Kaassoufflé", null, true, true, "Kaassoufflé", 2.20m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Grote loempia", null, true, true, "Loempia", 2.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 1, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Kipcorn", null, true, true, "Kipcorn", 2.30m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 2, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Broodje met kroket", null, true, true, "Broodje Kroket", 3.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, 2, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Broodje met frikandel", null, true, true, "Broodje Frikandel", 3.00m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, 2, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Broodje belegen kaas", null, true, true, "Broodje Kaas", 2.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, 2, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Broodje met salade, ei en tomaat", null, true, true, "Broodje Gezond", 4.00m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, 3, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Verse koffie", null, true, true, "Koffie", 2.00m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, 3, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Cappuccino", null, true, true, "Cappuccino", 2.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, 3, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Thee naar keuze", null, true, true, "Thee", 1.80m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, 3, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Warme chocolademelk", null, true, true, "Chocolademelk", 2.20m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, 4, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Coca-Cola 330ml", null, true, true, "Cola", 2.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, 4, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Fanta Orange 330ml", null, true, true, "Fanta", 2.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, 4, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Bruisend water 330ml", null, true, true, "Spa Rood", 2.00m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, 4, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Vers appelsap 200ml", null, true, true, "Appelsap", 2.00m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, 4, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Chocomel pakje 200ml", null, true, true, "Chocomel", 2.20m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 22, 5, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Ambachtelijke stroopwafel", null, true, true, "Stroopwafel", 1.00m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 23, 5, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Punt appeltaart", null, true, true, "Appeltaart", 3.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 24, 5, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Gevulde koek met amandelspijs", null, true, true, "Gevulde Koek", 1.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 25, 5, new DateTime(2026, 5, 4, 22, 0, 0, 0, DateTimeKind.Utc), "Chocoladereep", null, true, true, "Chocoladereep", 1.50m, new DateTime(2026, 5, 5, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
