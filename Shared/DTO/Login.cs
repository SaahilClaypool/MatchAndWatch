namespace Shared.DTO.Login {
    public record LoginDTO(string username, string password);
    public record LogoutDTO(string username);
    public record LoginResultDTO(bool Succeeded, string Token);
}
