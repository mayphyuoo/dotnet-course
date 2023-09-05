using Microsoft.AspNetCore.Mvc;

using DotnetAPI.Data;
using DotnetAPI.Models;
using DotnetAPI.Dtos;
using System.Reflection;
using System;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CompleteUserController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    public CompleteUserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers/{userId}/{isActive}")]
    public IEnumerable<CompleteUser> GetUsers(int userId, bool isActive)
    {
        string sql = @"EXEC TutorialAppSchema.spUsers_Get";
        string parameters = "";
        
        if(userId != 0)
        {
            parameters += ", @UserId=" + userId.ToString();
        }
        if (isActive)
        {
            parameters += ", @Active=" + isActive.ToString();
        }

        if(parameters.Length > 0)
        {
            sql += parameters.Substring(1);
        }

        IEnumerable<CompleteUser> users = _dapper.LoadData<CompleteUser>(sql);

        return users;
    }

    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(CompleteUser user)
    {
        string sql = @"EXEC TutorialAppSchema.spUser_Upsert
            @FirstName = '" + user.FirstName +
            "', @LastName = '" + user.LastName +
            "', @Email = '" + user.Email +
            "', @Gender = '" + user.Gender +
            "', @Active = '" + user.Active +
            "', @JobTitle = '" + user.JobTitle +
            "', @Department = '" + user.Department +
            "', @Salary = '" + user.Salary +
            "', @UserId = " + user.UserId;

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user.");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"EXEC TutorialAppSchema.spUser_Delete
            @UserId = " + userId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete user.");
    }
}
