using Microsoft.AspNetCore.Mvc;

using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using System.Reflection;
using System;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    private readonly DataContextEF _entityFramework;
    public UserEFController(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
    }

    [HttpGet("GetUsers")]
    public IEnumerable<CompleteUser> GetUsers()
    {
        IEnumerable<CompleteUser> users = _entityFramework.Users.ToList<CompleteUser>();

        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public CompleteUser GetSingleUser(int userId)
    {
        CompleteUser? user = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<CompleteUser> ();

        if (user != null)
        {
            return user;
        }

        throw new Exception("Failed to get user.");

    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(CompleteUser _user)
    {
        CompleteUser user = new CompleteUser();

        user.Active = _user.Active;
        user.FirstName = _user.FirstName;
        user.LastName = _user.LastName;
        user.Email = _user.Email;
        user.JobTitle = _user.JobTitle;
        user.Department = _user.Department;
        user.Salary = _user.Salary;
        user.AvgSalary = _user.AvgSalary;

        _entityFramework.Add(user);

        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to add user.");
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(CompleteUser _user)
    {
        CompleteUser? user = _entityFramework.Users
            .Where(u => u.UserId == _user.UserId)
            .FirstOrDefault<CompleteUser>();

        if (user != null)
        {
            user.Active = _user.Active;
            user.FirstName = _user.FirstName;
            user.LastName = _user.LastName;
            user.Email = _user.Email;
            user.JobTitle = _user.JobTitle;
            user.Department = _user.Department;
            user.Salary = _user.Salary;
            user.AvgSalary = _user.AvgSalary;

            if(_entityFramework.SaveChanges()>0)
            {
                return Ok();
            }
        }

        throw new Exception("Failed to update user.");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        CompleteUser? user = _entityFramework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<CompleteUser>();

        if (user != null)
        {
            _entityFramework.Users.Remove(user);

            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to delete user.");
        }

        throw new Exception("Failed to get user.");
    }
}
