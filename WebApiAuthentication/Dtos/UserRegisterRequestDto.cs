namespace WebApiAuthentication.Dtos;

public record UserRegisterRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

