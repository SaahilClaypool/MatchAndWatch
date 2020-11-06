namespace DTO.Login {
    public class LoginDTO {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginDTO(string username, string password) {
            this.Username = username;
            this.Password = password;
        }
    }
    public record LogoutDTO(string username);
    public record LoginResultDTO(bool Succeeded, string Token);
}
