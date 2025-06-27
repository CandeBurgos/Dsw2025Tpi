using Dsw2025Tpi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Dsw2025Tpi.Domain;



namespace Dsw2025Tpi.Data
{
    public class Dsw2025TpiContext : DbContext
    {
      
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public Dsw2025TpiContext(DbContextOptions<Dsw2025TpiContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Product
        modelBuilder.Entity<Product>(eb =>
        {
            eb.ToTable("Products");
            eb.HasKey(p => p.Id);
            
            eb.Property(p => p.Sku)
                .HasMaxLength(20)
                .IsRequired();
                
            eb.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();
                
            eb.Property(p => p.Description)
                .HasMaxLength(500);
                
            eb.Property(p => p.CurrentUnitPrice)
                .HasColumnType("decimal(18,2)");
                
            eb.Property(p => p.StockQuantity)
                .IsRequired();
                
            eb.Property(p => p.IsActive)
                .HasDefaultValue(true);
                
            // Índice único para SKU
            eb.HasIndex(p => p.Sku)
                .IsUnique();
        });

        // Configuración de Order
        modelBuilder.Entity<Order>(eb =>
        {
            eb.ToTable("Orders");
            eb.HasKey(o => o.Id);
            
            eb.Property(o => o.Date)
                .HasDefaultValueSql("GETUTCDATE()");
                
            eb.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(200);
                
            eb.Property(o => o.BillingAddress)
                .IsRequired()
                .HasMaxLength(200);
                
            eb.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");
                
            eb.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.PENDING);
                
            // Relación con OrderItems
            eb.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de OrderItem
        modelBuilder.Entity<OrderItem>(eb =>
        {
            eb.ToTable("OrderItems");
            eb.HasKey(oi => oi.Id);
            
            eb.Property(oi => oi.Quantity)
                .IsRequired();
                
            eb.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");
                
            eb.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(100);
                
            // Relación con Product
            eb.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
}
