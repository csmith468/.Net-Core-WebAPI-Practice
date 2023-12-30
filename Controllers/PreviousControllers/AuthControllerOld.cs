// using System.Data;
// using System.Security.Cryptography;
// using DotNetAPI.Data;
// using DotNetAPI.DTOs;
// using DotNetAPI.Helpers;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Data.SqlClient;

// namespace DotNetAPI.Controllers
// {

//     [Authorize]
//     [ApiController]
//     [Route("[controller]")]

//     public class AuthControllerOld : ControllerBase {
//         private readonly DataContextDapper _dapper;
//         private readonly AuthHelper _authHelper;

//         public AuthControllerOld(IConfiguration config) {
//             _dapper = new DataContextDapper(config);
//             _authHelper = new AuthHelper(config);
//         }

//         [AllowAnonymous]
//         [HttpPost("Register")]
//         public IActionResult Register(UserForRegistrationDTO userForRegistration) {
//             if (userForRegistration.Password == userForRegistration.PasswordConfirm) {

//                 string sqlCheckUserExists = "SELECT * FROM app.Auth WHERE Email = '" 
//                     + userForRegistration.Email + "'";
//                 IEnumerable<string> existingUsers = _dapper.Query<string>(sqlCheckUserExists);

//                 if (existingUsers.Count() == 0) {
//                     byte[] passwordSalt = new byte[128 / 8];
//                     using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
//                         rng.GetNonZeroBytes(passwordSalt);
//                     }
                    
//                     byte[] passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

//                     string sqlAddAuth = @"INSERT INTO app.Auth ([Email], [PasswordHash], [PasswordSalt])
//                             VALUES ('" + userForRegistration.Email +
//                             "', @PasswordHash, @PasswordSalt)";

//                     List<SqlParameter> sqlParameters = new List<SqlParameter>();

//                     SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
//                     passwordSaltParameter.Value = passwordSalt;

//                     SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
//                     passwordHashParameter.Value = passwordHash;

//                     sqlParameters.Add(passwordSaltParameter);
//                     sqlParameters.Add(passwordHashParameter);

//                     if (_dapper.ExecuteSqlWithListOfParameters(sqlAddAuth, sqlParameters)) {
//                         string sqlAddUser = @"
//                             INSERT INTO app.Users([FirstName], [LastName], [Email], [Gender], [Active])
//                             VALUES (
//                                 '" + userForRegistration.FirstName +
//                                 "', '" + userForRegistration.LastName +
//                                 "', '" + userForRegistration.Email +
//                                 "', '" + userForRegistration.Gender +
//                                 "', 1)";
                        
//                         if (_dapper.ExecuteBool(sqlAddUser)) {
//                             return Ok();
//                         }
//                         throw new Exception("Failed to add user.");
//                     }
//                     throw new Exception("Failed to register user.");
//                 }
//                 throw new Exception("User with this email already exists!");
//             }
//             throw new Exception("Passwords do not match!");
//         }

//         [AllowAnonymous]
//         [HttpPost("Login")]
//         public IActionResult Login(UserForLoginDTO userForLogin) {
//             string sqlForHashAndSalt = @"SELECT 
//                 [PasswordHash],
//                 [PasswordSalt] FROM app.Auth WHERE Email = '" +
//                 userForLogin.Email + "'";

//             UserForLoginConfirmationDTO userForConfirmation = _dapper
//                 .QuerySingle<UserForLoginConfirmationDTO>(sqlForHashAndSalt);

//             byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

//             // if (passwordHash == userForConfirmation.PasswordHash) // Won't work

//             for (int index = 0; index < passwordHash.Length; index++) {
//                 if (passwordHash[index] != userForConfirmation.PasswordHash[index]) {
//                     return StatusCode(401, "Incorrect password!");
//                 }
//             }

//             string userIdSql = @"
//                 SELECT UserId FROM app.Users WHERE Email = '" +
//                 userForLogin.Email + "'";

//             int userId = _dapper.QuerySingle<int>(userIdSql);

//             return Ok(new Dictionary<string, string> {
//                 {"token", _authHelper.CreateToken(userId)}
//             });
//         }

//         [HttpGet("RefreshToken")]
//         public string RefreshToken() {
//             string sqlUserId = @"
//                 SELECT UserId from app.Users where UserId = '" 
//                 + User.FindFirst("userId")?.Value +"'";

//             int userId = _dapper.QuerySingle<int>(sqlUserId);

//             return _authHelper.CreateToken(userId);
//         }


//     }
// }