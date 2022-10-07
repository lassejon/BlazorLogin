using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Login.DbContext;
using Login.Dtos;
using Login.Hashing;
using Login.Interfaces;
using Login.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Login.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LoginDbContext _loginDbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserController(LoginDbContext loginDbContext, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _loginDbContext = loginDbContext;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }
    
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody]CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Created = DateTime.Now;
            user.PasswordHash = _passwordHasher.Hash(userDto.Password);
                
            _loginDbContext.Users.Add(user);
            await _loginDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpGet("login")]
        public async Task<bool> IsPasswordCorrect([FromBody] LoginUserDto userDto)
        {
            var user = await _loginDbContext.Users.FirstOrDefaultAsync(u => userDto.Email == u!.Email);

            if (user is null) return false;
            
            var (verified, needsUpgrade) = _passwordHasher.Check(user.PasswordHash, userDto.Password);
            
            // todo: if needsUpgrade call _passwordHasher.UpgradeHash
            return verified;
        }

        [HttpGet("{id:int}")]
        public async Task<User?> GetUser(int id)
        {
            return await _loginDbContext.Users.FindAsync(id);
        }

        [HttpGet("all")]
        public ActionResult<User?[]> GetUsers()
        {
            // return new[]
            //     { new User() { Created = DateTime.Now, Email = "a@d.ck", Id = 10, Name = "Test", PasswordHash = "asd" } };
            return _loginDbContext.Users.ToArray();
        }
    }
}
