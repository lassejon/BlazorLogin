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

        [HttpDelete]
        public async Task<User?> DeleteUser(int id)
        {
            var user = new User { Id = id };
            _loginDbContext.Users.Remove(user);
            await _loginDbContext.SaveChangesAsync();

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody]CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Created = DateTime.Now;
            user.PasswordHash = _passwordHasher.Hash(userDto.Password);
                
            await _loginDbContext.Users.AddAsync(user);
            await _loginDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpGet("login")]
        public async Task<ActionResult<bool>> IsPasswordCorrect([FromBody] LoginUserDto userDto)
        {
            var user = await _loginDbContext.Users.FirstOrDefaultAsync(u => userDto.Email == u!.Email);

            if (user == null) return NotFound(false);
            
            var (verified, needsUpgrade) = _passwordHasher.Check(user.PasswordHash, userDto.Password);
            
            // todo: if needsUpgrade call _passwordHasher.UpgradeHash
            return Ok(verified);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _loginDbContext.Users.FindAsync(id);
            
            return user != null ? Ok(user) : NotFound(user);
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
