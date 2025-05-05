namespace managershop.Models
{
    public class LoginViewModel
    {
        // Tên người dùng hoặc email
        public string Username { get; set; }

        // Mật khẩu
        public string Password { get; set; }

        // Lưu mật khẩu hay không
        public bool RememberMe { get; set; }
    }
}
