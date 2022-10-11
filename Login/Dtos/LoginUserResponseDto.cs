namespace Login.Dtos;

public class LoginUserResponseDto
{
    public int Id { get; set; }
    public bool Verified { get; set; }
    public string Email { get; set; }
    public string? Name { get; set; }
}