using Microsoft.EntityFrameworkCore;
using managershop.Models;  // Đảm bảo bạn đã import các model của mình

namespace managershop.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor để khởi tạo DbContext với các tùy chọn cấu hình
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        // Định nghĩa các DbSet để đại diện cho các bảng trong cơ sở dữ liệu
        public DbSet<Product> Products { get; set; }
    }
}
