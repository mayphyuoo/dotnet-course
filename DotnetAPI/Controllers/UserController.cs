using Microsoft.AspNetCore.Mvc;

using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users";

        IEnumerable<User> users = _dapper.LoadData<User>(sql);

        return users;

        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users
                WHERE UserId = " + userId.ToString();

        User user = _dapper.LoadDataSingle<User>(sql);

        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.Users
            SET [FirstName] = '" + user.FirstName + 
            "', [LastName] = '" + user.LastName +
            "', [Email] = '" + user.Email +
            "', [Gender] = '" + user.Gender +
            "', [Active] = '" + user.Active +
            "' WHERE UserId = " + user.UserId;

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user.");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = @"
        INSERT INTO TutorialAppSchema.Users(
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
        ) VALUES (" +
            "'" + user.FirstName +
            "',  '" + user.LastName +
            "',  '" + user.Email +
            "',  '" + user.Gender +
            "',  '" + user.Active +
        "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to add user.");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.Users 
            WHERE UserId = " + userId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete user.");
    }

    /*  UserSalary Table
    */

    [HttpGet("GetUserSalaries")]
    public IEnumerable<UserSalary> GetUserSalaries()
    {
        string sql = @"
            SELECT [UserId],
                [Salary],
                [AvgSalary] 
            FROM TutorialAppSchema.UserSalary";

        IEnumerable<UserSalary> userSalaries = _dapper.LoadData<UserSalary>(sql);

        return userSalaries;
    }

    [HttpGet("GetSingleUserSalary/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [Salary],
                [AvgSalary] 
            FROM TutorialAppSchema.UserSalary
                WHERE UserId = " + userId.ToString();

        UserSalary user = _dapper.LoadDataSingle<UserSalary>(sql);

        return user;
    }

    [HttpPut("EditUserSalary")]
    public IActionResult EditUserSalary(UserSalary user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.UserSalary
            SET [Salary] = '" + user.Salary +
            "', [AvgSalary] = '" + user.AvgSalary +
            "' WHERE UserId = " + user.UserId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update UserSalary.");
    }

    [HttpPost("AddUserSalary")]
    public IActionResult AddUserSalary(UserSalary user)
    {
        string sql = @"
        INSERT INTO TutorialAppSchema.UserSalary(
            [UserId],
            [Salary],
            [AvgSalary]
        ) VALUES (" +
            "'" + user.UserId +
            "', '" + user.Salary +
            "', '" + user.AvgSalary +
        "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to add UserSalary.");
    }

    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.UserSalary 
            WHERE UserId = " + userId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete UserSalary.");
    }

    /*  UserJobInfo Table
    */

    [HttpGet("GetUserJobInfos")]
    public IEnumerable<UserJobInfo> GetUserJobInfos()
    {
        string sql = @"
            SELECT [UserId],
                [JobTitle],
                [Department] 
            FROM TutorialAppSchema.UserJobInfo";

        IEnumerable<UserJobInfo> userJobInfos = _dapper.LoadData<UserJobInfo>(sql);

        return userJobInfos;
    }

    [HttpGet("GetSingleUserJobInfo/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [JobTitle],
                [Department] 
            FROM TutorialAppSchema.UserJobInfo
                WHERE UserId = " + userId.ToString();

        UserJobInfo user = _dapper.LoadDataSingle<UserJobInfo>(sql);

        return user;
    }

    [HttpPut("EditUserJobInfo")]
    public IActionResult EditUserJobInfo(UserJobInfo user)
    {
        string sql = @"
        UPDATE TutorialAppSchema.UserJobInfo
            SET [JobTitle] = '" + user.JobTitle +
            "', [Department] = '" + user.Department +
            "' WHERE UserId = " + user.UserId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update userJobInfo.");
    }

    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfo user)
    {
        string sql = @"
        INSERT INTO TutorialAppSchema.UserJobInfo(
            [UserId],
            [JobTitle],
            [Department]
        ) VALUES (" +
            "'" + user.UserId +
            "', '" + user.JobTitle +
            "', '" + user.Department +
        "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to add userJobInfo.");
    }

    [HttpDelete("DeleteUserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        string sql = @"
        DELETE FROM TutorialAppSchema.UserJobInfo 
            WHERE UserId = " + userId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete userJobInfo.");
    }
}
