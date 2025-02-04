namespace LibraryApi.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public string RefreshToken { get; set; } = string.Empty;
        public DateTimeOffset RefreshTokenExpiryTime { get; set; }
    }
}
