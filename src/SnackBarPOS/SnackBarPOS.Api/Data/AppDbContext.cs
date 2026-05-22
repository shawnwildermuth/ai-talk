using Microsoft.EntityFrameworkCore;
using SnackBarPOS.Api.Entities;

namespace SnackBarPOS.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.IconEmoji).HasMaxLength(10);
        });

        // Product
        modelBuilder.Entity<Product>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.Price).HasColumnType("decimal(10,2)");
            e.Property(x => x.ImageUrl).HasMaxLength(500);
            e.HasOne(x => x.Category)
             .WithMany(x => x.Products)
             .HasForeignKey(x => x.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Order
        modelBuilder.Entity<Order>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.OrderNumber).HasMaxLength(20).IsRequired();
            e.Property(x => x.TotalAmount).HasColumnType("decimal(10,2)");
            e.Property(x => x.AmountTendered).HasColumnType("decimal(10,2)");
            e.Property(x => x.Change).HasColumnType("decimal(10,2)");
            e.Property(x => x.Notes).HasMaxLength(500);
            e.HasIndex(x => x.OrderNumber).IsUnique();
            e.Property(x => x.Status).HasConversion<string>();
            e.Property(x => x.PaymentMethod).HasConversion<string>();
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
            e.Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");
            e.Ignore(x => x.LineTotal);
            e.HasOne(x => x.Order)
             .WithMany(x => x.Items)
             .HasForeignKey(x => x.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Product)
             .WithMany(x => x.OrderItems)
             .HasForeignKey(x => x.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed data - typical Dutch snack bar items
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Snacks", Description = "Frituur snacks", IconEmoji = "🍟", SortOrder = 1 },
            new Category { Id = 2, Name = "Broodjes", Description = "Broodjes en sandwiches", IconEmoji = "🥪", SortOrder = 2 },
            new Category { Id = 3, Name = "Warme Dranken", Description = "Koffie, thee en chocolademelk", IconEmoji = "☕", SortOrder = 3 },
            new Category { Id = 4, Name = "Frisdranken", Description = "Koude dranken", IconEmoji = "🥤", SortOrder = 4 },
            new Category { Id = 5, Name = "Snoep & Koek", Description = "Snoep, koekjes en gebak", IconEmoji = "🍪", SortOrder = 5 }
        );

        modelBuilder.Entity<Product>().HasData(
            // Snacks
            new Product { Id = 1, Name = "Patat Klein", Price = 2.50m, CategoryId = 1, Description = "Kleine portie patat" },
            new Product { Id = 2, Name = "Patat Groot", Price = 3.50m, CategoryId = 1, Description = "Grote portie patat" },
            new Product { Id = 3, Name = "Kroket", Price = 2.00m, CategoryId = 1, Description = "Rundvleeskroket" },
            new Product { Id = 4, Name = "Frikandel", Price = 1.80m, CategoryId = 1, Description = "Klassieke frikandel" },
            new Product { Id = 5, Name = "Bitterballen (6 st)", Price = 4.50m, CategoryId = 1, Description = "6 stuks bitterballen" },
            new Product { Id = 6, Name = "Kaassoufflé", Price = 2.20m, CategoryId = 1, Description = "Kaassoufflé" },
            new Product { Id = 7, Name = "Loempia", Price = 2.50m, CategoryId = 1, Description = "Grote loempia" },
            new Product { Id = 8, Name = "Kipcorn", Price = 2.30m, CategoryId = 1, Description = "Kipcorn" },
            // Broodjes
            new Product { Id = 9, Name = "Broodje Kroket", Price = 3.50m, CategoryId = 2, Description = "Broodje met kroket" },
            new Product { Id = 10, Name = "Broodje Frikandel", Price = 3.00m, CategoryId = 2, Description = "Broodje met frikandel" },
            new Product { Id = 11, Name = "Broodje Kaas", Price = 2.50m, CategoryId = 2, Description = "Broodje belegen kaas" },
            new Product { Id = 12, Name = "Broodje Gezond", Price = 4.00m, CategoryId = 2, Description = "Broodje met salade, ei en tomaat" },
            // Warme dranken
            new Product { Id = 13, Name = "Koffie", Price = 2.00m, CategoryId = 3, Description = "Verse koffie" },
            new Product { Id = 14, Name = "Cappuccino", Price = 2.50m, CategoryId = 3, Description = "Cappuccino" },
            new Product { Id = 15, Name = "Thee", Price = 1.80m, CategoryId = 3, Description = "Thee naar keuze" },
            new Product { Id = 16, Name = "Chocolademelk", Price = 2.20m, CategoryId = 3, Description = "Warme chocolademelk" },
            // Frisdranken
            new Product { Id = 17, Name = "Cola", Price = 2.50m, CategoryId = 4, Description = "Coca-Cola 330ml" },
            new Product { Id = 18, Name = "Fanta", Price = 2.50m, CategoryId = 4, Description = "Fanta Orange 330ml" },
            new Product { Id = 19, Name = "Spa Rood", Price = 2.00m, CategoryId = 4, Description = "Bruisend water 330ml" },
            new Product { Id = 20, Name = "Appelsap", Price = 2.00m, CategoryId = 4, Description = "Vers appelsap 200ml" },
            new Product { Id = 21, Name = "Chocomel", Price = 2.20m, CategoryId = 4, Description = "Chocomel pakje 200ml" },
            // Snoep & koek
            new Product { Id = 22, Name = "Stroopwafel", Price = 1.00m, CategoryId = 5, Description = "Ambachtelijke stroopwafel" },
            new Product { Id = 23, Name = "Appeltaart", Price = 3.50m, CategoryId = 5, Description = "Punt appeltaart" },
            new Product { Id = 24, Name = "Gevulde Koek", Price = 1.50m, CategoryId = 5, Description = "Gevulde koek met amandelspijs" },
            new Product { Id = 25, Name = "Chocoladereep", Price = 1.50m, CategoryId = 5, Description = "Chocoladereep" }
        );
    }
}
