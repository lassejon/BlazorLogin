using System.Net;
using System.Web.Http;
using AutoMapper;
using Login.DbContext;
using Login.Dtos;
using Login.Interfaces;
using Login.Models;
using Microsoft.EntityFrameworkCore;

namespace Login.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LoginDbContext _loginDbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public UserRepository(LoginDbContext loginDbContext, IPasswordHasher passwordHasher, IMapper mapper)
    {
        _loginDbContext = loginDbContext;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }
    
    public async Task<User> DeleteUser(int id)
    {
        var user = await _loginDbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new Exception("User not found in table");
        }
        
        _loginDbContext.Users.Remove(user);
        await _loginDbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetUser(int id)
    {
        var user = await _loginDbContext.Users.FindAsync(id);

        return user;
    }

    public User[] GetUsers()
    {
        return _loginDbContext.Users.ToArray();
    }

    public async Task<LoginUserResponseDto?> LoginUser(LoginUserDto userDto)
    {
        var user = await _loginDbContext.Users.FirstOrDefaultAsync(u => userDto.Email == u!.Email);

        if (user == null) return null;
            
        var (verified, needsUpgrade) = _passwordHasher.Check(user.PasswordHash!, userDto.Password);

        var userResponse = _mapper.Map<LoginUserResponseDto>(user);
        userResponse.Verified = verified;
        
        // todo: if needsUpgrade call _passwordHasher.UpgradeHash
        return userResponse;
    }

    public async Task<User?> CreateUser(CreateUserDto userDto)
    {
        var userEmail = ((await _loginDbContext.Users.FirstOrDefaultAsync(u => userDto.Email == u!.Email))!).Email;
        
        if (userEmail != null)
        {
            return null;
        }
        
        var user = _mapper.Map<User>(userDto);
        user.Created = DateTime.Now;
        user.PasswordHash = _passwordHasher.Hash(userDto.Password);
                
        await _loginDbContext.Users.AddAsync(user);
        await _loginDbContext.SaveChangesAsync();

        return user;
    }
}