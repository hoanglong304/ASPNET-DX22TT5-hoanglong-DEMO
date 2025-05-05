namespace managershop.Models
{
    public class RegisterViewModel
    {
        // Tên người dùng
        public string Username { get; set; }

        // Mật khẩu
        public string Password { get; set; }

        // Nhập lại mật khẩu để xác nhận
        public string ConfirmPassword { get; set; }
    }
}
