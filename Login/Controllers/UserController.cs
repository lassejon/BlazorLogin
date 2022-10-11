using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LoginDbContext _loginDbContext;
        private readonly IUserRepository _repository;

        public UserController(LoginDbContext loginDbContext, IUserRepository repository)
        {
            _loginDbContext = loginDbContext;
            _repository = repository;
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            User user;
            
            try
            {
                user = await _repository.DeleteUser(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok(user);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<ActionResult<User>> PostUser([Microsoft.AspNetCore.Mvc.FromBody]CreateUserDto userDto)
        {
            var user = await _repository.CreateUser(userDto);

            if (user == null)
            {
                return BadRequest("Email exists in db");
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("login")]
        public async Task<ActionResult<LoginUserResponseDto>> IsPasswordCorrect([Microsoft.AspNetCore.Mvc.FromBody] LoginUserDto userDto)
        {
            var user = await _repository.LoginUser(userDto);

            if (user == null) return NotFound();

            // todo: if needsUpgrade call _passwordHasher.UpgradeHash
            return Ok(user);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _repository.GetUser(id);
            
            return user != null ? Ok(user) : NotFound(user);
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("all")]
        public ActionResult<User?[]> GetUsers()
        {
            // return new[]
            //     { new User() { Created = DateTime.Now, Email = "a@d.ck", Id = 10, Name = "Test", PasswordHash = "asd" } };
            return _repository.GetUsers();
        }
    }
}
