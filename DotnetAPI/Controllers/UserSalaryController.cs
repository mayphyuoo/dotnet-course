using Microsoft.AspNetCore.Mvc;

using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserSalaryController : ControllerBase
{
    DataContextDapper _dapper;
    public UserSalaryController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

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

        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
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

        if(_dapper.ExecuteSql(sql))
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
    public IActionResult DeleteUser(int userId)
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
}
