using managershop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace managershop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductSize>()
                .HasKey(ps => new { ps.ProductId, ps.Size });

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.OrderId, od.ProductId, od.Size });

            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(c => c.User)
                    .WithMany(u => u.CartItems)  // Khai báo collection CartItems trong User
                    .HasForeignKey(c => c.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Product)
                    .WithMany() // Nếu Product có collection CartItems, sửa lại tương tự
                    .HasForeignKey(c => c.ProductId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
