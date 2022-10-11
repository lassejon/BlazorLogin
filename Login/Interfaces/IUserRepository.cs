using Login.Dtos;
using Login.Models;

namespace Login.Interfaces;

public interface IUserRepository
{
    public Task<User> DeleteUser(int id);
    public Task<User?> GetUser(int id);
    public User[] GetUsers();
    public Task<LoginUserResponseDto?> LoginUser(LoginUserDto userDto);
    public Task<User?> CreateUser(CreateUserDto userDto);
}