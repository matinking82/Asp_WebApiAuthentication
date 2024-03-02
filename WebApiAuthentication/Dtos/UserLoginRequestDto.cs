namespace WebApiAuthentication.Dtos;

public record UserLoginRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}