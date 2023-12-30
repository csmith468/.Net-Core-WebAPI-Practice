// using DotNetAPI.Data;
// using DotNetAPI.DTOs;
// using DotNetAPI.Models;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace DotNetAPI.Controllers {

//     [Authorize]
//     [ApiController]
//     [Route("[controller]")]
//     public class PostControllerOld : ControllerBase {
//         private readonly DataContextDapper _dapper;

//         public PostControllerOld(IConfiguration config) {
//             _dapper = new DataContextDapper(config);
//         }

//         [HttpGet("GetPosts")]
//         public IEnumerable<Post> GetPosts() {
//             string sql = @"
//                 SELECT [PostId],
//                     [UserId],
//                     [PostTitle],
//                     [PostContent],
//                     [PostCreated],
//                     [PostUpdated] 
//                 FROM app.Posts";
            
//             return _dapper.Query<Post>(sql);
//         }

//         [HttpGet("GetPostSingle/{postId}")]
//         public Post GetPostSingle(int postId) {
//             string sql = @"
//                 SELECT [PostId],
//                     [UserId],
//                     [PostTitle],
//                     [PostContent],
//                     [PostCreated],
//                     [PostUpdated] 
//                 FROM app.Posts
//                 WHERE PostId = " + postId.ToString();
                
//             return _dapper.QuerySingle<Post>(sql);
//         }

//         [HttpGet("GetPostsByUser/{userId}")]
//         public IEnumerable<Post> GetPostsByUser(int userId) {
//             string sql = @"
//                 SELECT [PostId],
//                     [UserId],
//                     [PostTitle],
//                     [PostContent],
//                     [PostCreated],
//                     [PostUpdated] 
//                 FROM app.Posts
//                 WHERE UserId = " + userId.ToString();
                
//             return _dapper.Query<Post>(sql);
//         }

//         [HttpGet("GetMyPosts")]
//         public IEnumerable<Post> GetMyPosts() {
//             string sql = @"
//                 SELECT [PostId],
//                     [UserId],
//                     [PostTitle],
//                     [PostContent],
//                     [PostCreated],
//                     [PostUpdated] 
//                 FROM app.Posts
//                 WHERE UserId = " + this.User.FindFirst("userId")?.Value;
//             // this.User is the user from the ControllerBase, not from User.cs
                
//             return _dapper.Query<Post>(sql);
//         }

//         [HttpPost("AddPost")]
//         public IActionResult AddPost(PostToAddDTO postToAdd) {
//             string sql = @"
//                 INSERT INTO app.Posts(
//                     [UserId], [PostTitle], [PostContent], [PostCreated], [PostUpdated]) 
//                 VALUES (" + this.User.FindFirst("userId")?.Value +
//                     ",'" + postToAdd.PostTitle + 
//                     "','" + postToAdd.PostContent + 
//                     "', GETDATE(), GETDATE())";

//             if (_dapper.ExecuteBool(sql)) {
//                 return Ok();
//             }

//             throw new Exception("Failed to create new post.");
//         }

//         [HttpPost("EditPost")]
//         public IActionResult EditPost(PostToEditDTO postToEdit) {
//             string sql = @"
//                 UPDATE app.Posts
//                 SET PostTitle = '" + postToEdit.PostTitle + 
//                     "', PostContent = '" + postToEdit.PostContent + 
//                     @"', PostUpdated = GETDATE()
//                 WHERE UserId = " + this.User.FindFirst("userId")?.Value + 
//                 "AND PostId = " + postToEdit.PostId.ToString();

//             if (_dapper.ExecuteBool(sql)) {
//                 return Ok();
//             }

//             throw new Exception("Failed to edit post.");
//         }

        
//         [HttpDelete("DeletePost/{postId}")]
//         public IActionResult DeletePost(int postId) {
//             string sql = @"
//                 DELETE FROM app.Posts 
//                 WHERE PostId = " + postId.ToString()+
//                     "AND UserId = " + this.User.FindFirst("userId")?.Value;
            
//             if (_dapper.ExecuteBool(sql)) {
//                 return Ok();
//             }

//             throw new Exception("Failed to delete post!");
//         }

//         [HttpGet("PostsBySearch/{searchParam}")]
//         public IEnumerable<Post> PostsBySearch(string searchParam) {
//             string sql = @"
//                 SELECT [PostId],
//                     [UserId],
//                     [PostTitle],
//                     [PostContent],
//                     [PostCreated],
//                     [PostUpdated] 
//                 FROM app.Posts
//                 WHERE [PostTitle] like '%" + searchParam + "%'" +
//                 "OR [PostContent] like '%" + searchParam + "%'";
            
//             return _dapper.Query<Post>(sql);
//         }
//     }
// }