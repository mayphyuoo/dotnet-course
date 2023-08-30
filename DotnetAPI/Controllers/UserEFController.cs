using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using AutoMapper;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IMapper _mapper;
    public UserEFController(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
        }));
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();

        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        User? user = _entityFramework.Users
             .Where(u => u.UserId == userId)
             .FirstOrDefault<User>();

        if(user != null)
        {
            return user;
        }

        throw new Exception("Failed to Get User");
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User _user)
    {
        User? user = _entityFramework.Users
             .Where(u => u.UserId == _user.UserId)
             .FirstOrDefault<User>();

        if (user != null)
        {
            user.FirstName = _user.FirstName;
            user.LastName = _user.LastName;
            user.Email = _user.Email;
            user.Gender = _user.Gender;
            user.Active = _user.Active;
            if(_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Update user");
        }

        throw new Exception("Failed to Get User");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto _user)
    {
        User user = _mapper.Map<User>(_user);

        _entityFramework.Add(user);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to Add user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? user = _entityFramework.Users
             .Where(u => u.UserId == userId)
             .FirstOrDefault<User>();

        if (user != null)
        {
            _entityFramework.Users.Remove(user);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }

            throw new Exception("Failed to Delete user");
        }

        throw new Exception("Failed to Get User");
    }
}
