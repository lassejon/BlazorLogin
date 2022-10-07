using Login.Models;

namespace Login.Dtos;

public class CreateUserDto
{
    public string? Name { get; set; }

    public string? Email { get; set; }
    
    public string Password { get; set; }
}