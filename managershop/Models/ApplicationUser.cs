// ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

namespace managershop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        // Thêm các thuộc tính khác nếu cần
    }
}
