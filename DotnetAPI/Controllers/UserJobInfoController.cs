using Microsoft.AspNetCore.Mvc;

using DotnetAPI.Data;
using DotnetAPI.Models;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserJobInfoController : ControllerBase
{
    DataContextDapper _dapper;
    public UserJobInfoController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

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

        // return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        // {
        //     Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //     TemperatureC = Random.Shared.Next(-20, 55),
        //     Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        // })
        // .ToArray();
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

        if(_dapper.ExecuteSql(sql))
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
    public IActionResult DeleteUser(int userId)
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
