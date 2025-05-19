// ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace managershop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Id { get; set; }   // Đảm bảo là int
        // Thêm các thuộc tính khác nếu cần
    }
}
