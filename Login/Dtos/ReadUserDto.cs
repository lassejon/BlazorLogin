namespace Login.Dtos;

public class ReadUserDto
{
    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? Created { get; set; }
}