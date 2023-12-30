// using AutoMapper;
// using DotNetAPI.Data;
// using DotNetAPI.DTOs;
// using DotNetAPI.Models;
// using Microsoft.AspNetCore.Mvc;

// namespace DotNetAPI.Controllers;

// [ApiController]
// [Route("[controller]")]
// public class UserEFController : ControllerBase {

//     // DataContextEF _entityFramework;
//     IUserRepository _userRepository;
//     IMapper _mapper;

//     public UserEFController(IConfiguration config, IUserRepository userRepository) {
//         // _entityFramework = new DataContextEF(config);
//         _userRepository = userRepository;
//         _mapper = new Mapper(new MapperConfiguration(cfg => {
//             cfg.CreateMap<UserDTO, User>();
//         }));
//     }

//     [HttpGet("GetUsers")]
//     public IEnumerable<User> GetUsers() {
//         IEnumerable<User> users = _userRepository.GetUsers();
//         return users;
//     }

//     [HttpGet("GetSingleUser/{userId}")]
//     public User GetSingleUser(int userId) {
//         return _userRepository.GetSingleUser(userId);
//     }

//     [HttpPut("EditUser")]
//     public IActionResult EditUser(User user) {
//         // User? userDb = _entityFramework.Users
//         //     .Where(u => u.UserId == user.UserId)
//         //     .FirstOrDefault<User>();
//         User? userDb = _userRepository.GetSingleUser(user.UserId);

//         if (userDb != null) {
//             userDb.FirstName = user.FirstName;
//             userDb.LastName = user.LastName;
//             userDb.Email = user.Email;
//             userDb.Gender = user.Gender;
//             userDb.Active = user.Active;
//             if (_userRepository.SaveChanges()) {
//                 return Ok();
//             }
//         }

//         throw new Exception("Failed to update user.");
//     }

//     [HttpPost("AddUser")]
//     public IActionResult AddUser(UserDTO user) {
//         User userDb = _mapper.Map<User>(user);
//         _userRepository.Add<User>(userDb);

//         if (_userRepository.SaveChanges()) {
//             return Ok();
//         }

//         throw new Exception("Failed to add user.");
//     }

//     [HttpDelete("DeleteUser/{userId}")]
//     public IActionResult DeleteUser(int userId) {
//         // User? userDb = _entityFramework.Users
//         //     .Where(u => u.UserId == userId)
//         //     .FirstOrDefault<User>();
//         User? userDb = _userRepository.GetSingleUser(userId);

//         if (userDb != null) {
//             _userRepository.Remove<User>(userDb);
//             if (_userRepository.SaveChanges()) {
//                 return Ok();
//             }
//         }

//         throw new Exception("Failed to delete user.");
//     }

    
//     [HttpGet("GetUserSalary/{userId}")]
//     public UserSalary GetUserSalaryEF(int userId) {
//         // return _entityFramework.UserSalary
//         //     .Where(u => u.UserId == userId)
//         //     .ToList();
//         return _userRepository.GetSingleUserSalary(userId);
//     }

//     [HttpPost("AddUserSalary")]
//     public IActionResult PostUserSalaryEf(UserSalary userForInsert) {
//         _userRepository.Add<UserSalary>(userForInsert);
//         if (_userRepository.SaveChanges()) {
//             return Ok();
//         }
//         throw new Exception("Adding UserSalary failed on save");
//     }


//     [HttpPut("EditUserSalary")]
//     public IActionResult PutUserSalaryEf(UserSalary userForUpdate) {
//         // UserSalary? userToUpdate = _entityFramework.UserSalary
//         //     .Where(u => u.UserId == userForUpdate.UserId)
//         //     .FirstOrDefault();
//         UserSalary? userToUpdate = _userRepository.GetSingleUserSalary(userForUpdate.UserId);

//         if (userToUpdate != null) {
//             _mapper.Map(userForUpdate, userToUpdate);
//             if (_userRepository.SaveChanges()) {
//                 return Ok();
//             }
//             throw new Exception("Updating UserSalary failed on save");
//         }
//         throw new Exception("Failed to find UserSalary to Update");
//     }


//     [HttpDelete("DeleteUserSalary/{userId}")]
//     public IActionResult DeleteUserSalaryEf(int userId) {
//         // UserSalary? userToDelete = _entityFramework.UserSalary
//         //     .Where(u => u.UserId == userId)
//         //     .FirstOrDefault();
//         UserSalary? userToDelete = _userRepository.GetSingleUserSalary(userId);

//         if (userToDelete != null) {
//             _userRepository.Remove<UserSalary>(userToDelete);
//             if (_userRepository.SaveChanges()) {
//                 return Ok();
//             }
//             throw new Exception("Deleting UserSalary failed on save");
//         }
//         throw new Exception("Failed to find UserSalary to delete");
//     }


//     [HttpGet("GetUserJobInfo/{userId}")]
//     public UserJobInfo GetUserJobInfoEF(int userId) {
//         // return _entityFramework.UserJobInfo
//         //     .Where(u => u.UserId == userId)
//         //     .ToList();
//         return _userRepository.GetSingleUserJobInfo(userId);
//     }

//     [HttpPost("AddUserJobInfo")]
//     public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert) {
//         _userRepository.Add<UserJobInfo>(userForInsert);
//         if (_userRepository.SaveChanges()) {
//             return Ok();
//         }
//         throw new Exception("Adding UserJobInfo failed on save");
//     }


//     [HttpPut("EditUserJobInfo")]
//     public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate) {
//         // UserJobInfo? userToUpdate = _entityFramework.UserJobInfo
//         //     .Where(u => u.UserId == userForUpdate.UserId)
//         //     .FirstOrDefault();
//         UserJobInfo? userToUpdate = _userRepository.GetSingleUserJobInfo(userForUpdate.UserId);

//         if (userToUpdate != null) {
//             _mapper.Map(userForUpdate, userToUpdate);
//             if (_userRepository.SaveChanges()) {
//                 return Ok();
//             }
//             throw new Exception("Updating UserJobInfo failed on save");
//         }
//         throw new Exception("Failed to find UserJobInfo to Update");
//     }


//     [HttpDelete("DeleteUserJobInfo/{userId}")]
//     public IActionResult DeleteUserJobInfoEf(int userId) {
//         // UserJobInfo? userToDelete = _entityFramework.UserJobInfo
//         //     .Where(u => u.UserId == userId)
//         //     .FirstOrDefault();
//         UserJobInfo? userToDelete = _userRepository.GetSingleUserJobInfo(userId);

//         if (userToDelete != null) {
//             _userRepository.Remove<UserJobInfo>(userToDelete);
//             if (_userRepository.SaveChanges()) {
//                 return Ok();
//             }
//             throw new Exception("Deleting UserJobInfo failed on save");
//         }
//         throw new Exception("Failed to find UserJobInfo to delete");
//     }
// }
