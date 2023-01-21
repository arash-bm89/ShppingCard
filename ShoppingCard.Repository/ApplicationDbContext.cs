using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCard.Domain.Models;
using ShoppingCard.Repository.EntityConfigurations;

namespace ShoppingCard.Repository;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Basket> Baskets { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<BasketProduct> BasketProducts { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new BasketProductConfiguration());
        modelBuilder.ApplyConfiguration(new BasketConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());
    }
}