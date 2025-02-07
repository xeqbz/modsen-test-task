namespace LibraryApi.Application.DTOs
{
    public class RegisterRequest
    {
        public string UsernameReg { get; set; } = string.Empty;
        public string PasswordReg { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
