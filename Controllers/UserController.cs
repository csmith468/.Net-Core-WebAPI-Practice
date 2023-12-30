using System.Data;
using Dapper;
using DotNetAPI.Data;
using DotNetAPI.DTOs;
using DotNetAPI.Helpers;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers;

// [Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase {

    private readonly DataContextDapper _dapper;
    private readonly ReusableSql _reusableSql;

    public UserController(IConfiguration config) {
        _dapper = new DataContextDapper(config);
        _reusableSql = new ReusableSql(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection() {
        return _dapper.QuerySingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers/{userId}/{isActive}")]
    public IEnumerable<UserComplete> GetUsers(int userId, bool isActive) {
        string sql = @"EXECUTE [app].[spUser_Get]";
        string stringParameters = "";
        DynamicParameters sqlParameters = new DynamicParameters();
        
        if (userId != 0) {
            stringParameters += ", @UserId = @UserIdParameter";
            sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
        }
        if (isActive) {
            stringParameters += ", @Active = @ActiveParameter";
            sqlParameters.Add("@ActiveParameter", isActive, DbType.Boolean);
        }

        if (stringParameters.Length > 0) {
            sql += stringParameters.Substring(1);// parameters.Length);
        }
         
        IEnumerable<UserComplete> users = _dapper.QueryWithParameters<UserComplete>(sql, sqlParameters);
        return users;
    }

    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(UserComplete userComplete) {
        if (_reusableSql.UpsertUser(userComplete)) {
            return Ok();  
        }
        throw new Exception("Failed to update user.");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId) {
        string sql = @"
            EXEC app.spUser_Delete
            @UserId = " + userId.ToString();

        if (_dapper.ExecuteBool(sql)) {
            return Ok();
        }
        throw new Exception("Failed to delete user.");
    }
}
