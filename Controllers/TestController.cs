using System.Data;
using Dapper;
using DotNetAPI.Data;
using DotNetAPI.DTOs;
using DotNetAPI.Helpers;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase {

    private readonly DataContextDapper _dapper;

    public TestController(IConfiguration config) {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection() {
        return _dapper.QuerySingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet]
    public string Test() {
        return "Your app is running.";
    }
}
