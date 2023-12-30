// using DotNetAPI.Data;
// using DotNetAPI.DTOs;
// using DotNetAPI.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace DotNetAPI.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class UserControllerOld : ControllerBase {

//     DataContextDapper _dapper;

//     public UserControllerOld(IConfiguration config) {
//         _dapper = new DataContextDapper(config);
//     }

//     [HttpGet("TestConnection")]
//     public DateTime TestConnection() {
//         return _dapper.QuerySingle<DateTime>("SELECT GETDATE()");
//     }

//     [HttpGet("GetUsers")]
//     public IEnumerable<User> GetUsers() {
//         string sql = @"
//             SELECT [UserId],
//                 [FirstName],
//                 [LastName],
//                 [Email],
//                 [Gender],
//                 [Active] 
//             FROM app.Users";
//         IEnumerable<User> users = _dapper.Query<User>(sql);
//         return users;
//     }

//     [HttpGet("GetSingleUser/{userId}")]
//     public User GetSingleUser(int userId) {
//         string sql = @"
//             SELECT [UserId],
//                 [FirstName],
//                 [LastName],
//                 [Email],
//                 [Gender],
//                 [Active] 
//             FROM app.Users
//             WHERE [UserId] = " + userId.ToString();
//         return _dapper.QuerySingle<User>(sql);
//     }

//     [HttpPut("EditUser")]
//     public IActionResult EditUser(User user) {
//         string sql = @"
//             UPDATE app.Users
//             SET [FirstName] = '" + user.FirstName + 
//                 "',[LastName] = '" + user.LastName + 
//                 "',[Email] = '" + user.Email + 
//                 "',[Gender] = '" + user.Gender + 
//                 "',[Active] = '" + user.Active +
//             "' WHERE UserId=" + user.UserId;
//         if (_dapper.ExecuteBool(sql)) {
//             return Ok();  
//         }
//         throw new Exception("Failed to update user.");
//     }
    
//     [HttpPut("EditUserActive")]
//     public IActionResult EditUserActive(User user) {
//         string sql = @"
//             UPDATE app.Users
//             SET [Active] = '" + user.Active +
//             "' WHERE UserId=" + user.UserId;
//         if (_dapper.ExecuteBool(sql)) {
//             return Ok();  
//         }
//         throw new Exception("Failed to update user.");
//     }

//     [HttpPost("AddUser")]
//     public IActionResult AddUser(UserDTO user) {
//         string sql = @"
//             INSERT INTO app.Users([FirstName], [LastName], [Email], [Gender], [Active])
//             VALUES (
//                 '" + user.FirstName +
//                 "', '" + user.LastName +
//                 "', '" + user.Email +
//                 "', '" + user.Gender +
//                 "', '" + user.Active +
//             "')";
//         Console.WriteLine(sql);
//         if (_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Failed to add user.");
//     }

//     [HttpDelete("DeleteUser/{userId}")]
//     public IActionResult DeleteUser(int userId) {
//         string sql = @"
//             DELETE FROM app.Users 
//             WHERE [UserId] = " + userId.ToString();

//         if (_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Failed to delete user.");
//     }

//     [HttpGet("GetUserSalaries")]
//     public IEnumerable<UserSalary> GetUserSalaries() {
//         string sql = @"
//             SELECT [UserId],
//                 [Salary]
//             FROM app.UserSalary";
//         IEnumerable<UserSalary> userSalaries = _dapper.Query<UserSalary>(sql);
//         return userSalaries;
//     }


//     [HttpGet("GetUserSalary/{userId}")]
//     public IEnumerable<UserSalary> GetUserSalary(int userId) {
//         return _dapper.Query<UserSalary>(@"
//             SELECT UserSalary.UserId
//                     , UserSalary.Salary
//             FROM  app.UserSalary
//                 WHERE UserId = " + userId.ToString());
//     }

//     [HttpPost("AddUserSalary")]
//     public IActionResult PostUserSalary(UserSalary userSalaryForInsert) {
//         string sql = @"
//             INSERT INTO app.UserSalary (
//                 UserId,
//                 Salary
//             ) VALUES (" + userSalaryForInsert.UserId.ToString()
//                 + ", " + userSalaryForInsert.Salary
//                 + ")";

//         if (_dapper.Execute(sql) > 0) {
//             return Ok(userSalaryForInsert);
//         }
//         throw new Exception("Adding User Salary failed on save");
//     }

//     [HttpPut("EditUserSalary")]
//     public IActionResult PutUserSalary(UserSalary userSalaryForUpdate) {
//         string sql = "UPDATE app.UserSalary SET Salary=" 
//             + userSalaryForUpdate.Salary
//             + " WHERE UserId=" + userSalaryForUpdate.UserId.ToString();

//         if (_dapper.ExecuteBool(sql)) {
//             return Ok(userSalaryForUpdate);
//         }
//         throw new Exception("Updating User Salary failed on save");
//     }

//     [HttpDelete("DeleteUserSalary/{userId}")]
//     public IActionResult DeleteUserSalary(int userId) {
//         string sql = "DELETE FROM app.UserSalary WHERE UserId=" + userId.ToString();

//         if (_dapper.ExecuteBool(sql)) {
//             return Ok();
//         }
//         throw new Exception("Deleting User Salary failed on save");
//     }

//     [HttpGet("GetAllUserJobInfo")]
//     public IEnumerable<UserJobInfo> GetAllUserJobInfo() {
//         string sql = @"
//             SELECT [UserId],
//                 [JobTitle],
//                 [Department]
//             FROM app.UserJobInfo";
//         IEnumerable<UserJobInfo> userJobInfo = _dapper.Query<UserJobInfo>(sql);
//         return userJobInfo;
//     }

//     [HttpGet("GetUserJobInfo/{userId}")]
//     public IEnumerable<UserJobInfo> GetUserJobInfo(int userId) {
//         return _dapper.Query<UserJobInfo>(@"
//             SELECT  UserJobInfo.UserId
//                     , UserJobInfo.JobTitle
//                     , UserJobInfo.Department
//             FROM  app.UserJobInfo
//                 WHERE UserId = " + userId.ToString());
//     }

//     [HttpPost("AddUserJobInfo")]
//     public IActionResult PostUserJobInfo(UserJobInfo userJobInfoForInsert) {
//         string sql = @"
//             INSERT INTO app.UserJobInfo (
//                 UserId,
//                 Department,
//                 JobTitle
//             ) VALUES (" + userJobInfoForInsert.UserId
//                 + ", '" + userJobInfoForInsert.Department
//                 + "', '" + userJobInfoForInsert.JobTitle
//                 + "')";

//         if (_dapper.ExecuteBool(sql)) {
//             return Ok(userJobInfoForInsert);
//         }
//         throw new Exception("Adding User Job Info failed on save");
//     }

//     [HttpPut("EditUserJobInfo")]
//     public IActionResult PutUserJobInfo(UserJobInfo userJobInfoForUpdate) {
//         string sql = "UPDATE app.UserJobInfo SET Department='" 
//             + userJobInfoForUpdate.Department
//             + "', JobTitle='"
//             + userJobInfoForUpdate.JobTitle
//             + "' WHERE UserId=" + userJobInfoForUpdate.UserId.ToString();

//         if (_dapper.ExecuteBool(sql))
//         {
//             return Ok(userJobInfoForUpdate);
//         }
//         throw new Exception("Updating User Job Info failed on save");
//     }

//     [HttpDelete("DeleteUserJobInfo/{userId}")]
//     public IActionResult DeleteUserJobInfo(int userId) {
//         string sql = @"
//             DELETE FROM app.UserJobInfo 
//                 WHERE UserId = " + userId.ToString();
        
//         Console.WriteLine(sql);

//         if (_dapper.ExecuteBool(sql)) {
//             return Ok();
//         } 

//         throw new Exception("Failed to Delete User");
//     }

// }
