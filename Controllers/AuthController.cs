using System.Data;
using System.Security.Cryptography;
using AutoMapper;
using Dapper;
using DotNetAPI.Data;
using DotNetAPI.DTOs;
using DotNetAPI.Helpers;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotNetAPI.Controllers
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class AuthController : ControllerBase {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;
        private readonly ReusableSql _reusableSql;
        private readonly IMapper _mapper;

        public AuthController(IConfiguration config) {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
            _reusableSql = new ReusableSql(config);
            _mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.CreateMap<UserCompleteForRegistrationDTO, UserComplete>();
            }));
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserCompleteForRegistrationDTO userForRegistration) {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm) {

                string sqlCheckUserExists = "SELECT * FROM app.Auth WHERE Email = '" 
                    + userForRegistration.Email + "'";
                IEnumerable<string> existingUsers = _dapper.Query<string>(sqlCheckUserExists);

                if (existingUsers.Count() == 0) {
                    
                    UserForLoginDTO userForSetPassword = new UserForLoginDTO() {
                        Email = userForRegistration.Email,
                        Password = userForRegistration.Password
                    };

                    if (_authHelper.SetPassword(userForSetPassword)) {
                        UserComplete userComplete = _mapper.Map<UserComplete>(userForRegistration);
                        userComplete.Active = true;
                        
                        if (_reusableSql.UpsertUser(userComplete)) {
                            return Ok();
                        }
                        throw new Exception("Failed to add user.");
                    }
                    throw new Exception("Failed to register user.");
                }
                throw new Exception("User with this email already exists!");
            }
            throw new Exception("Passwords do not match!");
        }

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword(UserForLoginDTO userForResetPassword) {
            if (_authHelper.SetPassword(userForResetPassword)) {
                return Ok();
            }
            throw new Exception("Failed to update password.");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDTO userForLogin) {
            string sqlForHashAndSalt = @"
                EXEC app.spLoginConfirmation_Get
                @Email = @EmailParam";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@EmailParam", userForLogin.Email, DbType.String);

            UserForLoginConfirmationDTO userForConfirmation = _dapper
                .QuerySingleWithParameters<UserForLoginConfirmationDTO>(sqlForHashAndSalt, sqlParameters);

            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

            // if (passwordHash == userForConfirmation.PasswordHash) // Won't work

            for (int index = 0; index < passwordHash.Length; index++) {
                if (passwordHash[index] != userForConfirmation.PasswordHash[index]) {
                    return StatusCode(401, "Incorrect password!");
                }
            }

            string userIdSql = @"
                SELECT UserId FROM app.Users WHERE Email = '" +
                userForLogin.Email + "'";

            int userId = _dapper.QuerySingle<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken() {
            string sqlUserId = @"
                SELECT UserId from app.Users where UserId = '" 
                + User.FindFirst("userId")?.Value +"'";

            int userId = _dapper.QuerySingle<int>(sqlUserId);

            return _authHelper.CreateToken(userId);
        }


    }
}