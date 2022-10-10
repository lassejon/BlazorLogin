using System.ComponentModel.DataAnnotations;
using Login.Models;

namespace Login.Dtos;

public class CreateUserDto
{
    public string? Name { get; set; }

    [Required]
    public string? Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}