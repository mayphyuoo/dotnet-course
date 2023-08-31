using Microsoft.AspNetCore.Mvc;

using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using AutoMapper;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    IUserRepository _userRepository;
    IMapper _mapper;
    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
        }));
    }

    [HttpGet("GetUsersEf")]
    public IEnumerable<User> GetUsersEf()
    {
        IEnumerable<User> users = _userRepository.GetUsersEf();

        return users;
    }

    [HttpGet("GetSingleUserEf/{userId}")]
    public User GetSingleUserEf(int userId)
    {
        return _userRepository.GetSingleUserEf(userId);
    }

    [HttpPut("EditUserEf")]
    public IActionResult EditUserEf(User _user)
    {
        User? user = _userRepository.GetSingleUserEf(_user.UserId);

        if (user != null)
        {
            user.FirstName = _user.FirstName;
            user.LastName = _user.LastName;
            user.Email = _user.Email;
            user.Gender = _user.Gender;
            user.Active = _user.Active;
            if(_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Update user");
        }

        throw new Exception("Failed to Get User");
    }

    [HttpPost("AddUserEf")]
    public IActionResult AddUserEf(UserToAddDto _user)
    {
        User user = _mapper.Map<User>(_user);

        _userRepository.AddEntity<User>(user);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to Add user");
    }

    [HttpDelete("DeleteUserEf/{userId}")]
    public IActionResult DeleteUserEf(int userId)
    {
        User? user = _userRepository.GetSingleUserEf(userId);

        if (user != null)
        {
            _userRepository.RemoveEntity<User>(user);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Failed to Delete user");
        }

        throw new Exception("Failed to Get User");
    }
}
