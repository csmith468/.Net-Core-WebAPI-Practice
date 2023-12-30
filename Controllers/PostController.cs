using System.Data;
using Dapper;
using DotNetAPI.Data;
using DotNetAPI.DTOs;
using DotNetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI.Controllers {

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase {
        private readonly DataContextDapper _dapper;

        public PostController(IConfiguration config) {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("GetPosts/{postId}/{userId}/{searchParam}")]
        public IEnumerable<Post> GetPosts(int postId = 0, int userId = 0, string searchParam = "None") {
            string sql = @"EXEC app.spPost_Get";
            string stringParameters = "";
            DynamicParameters sqlParameters = new DynamicParameters();
            
            if (postId != 0) {
                stringParameters += ", @PostId=@PostIdParameter";
                sqlParameters.Add("@PostIdParameter", postId, DbType.Int32);
            }
            if (userId != 0) {
                stringParameters += ", @UserId=@UserIdParameter";
                sqlParameters.Add("@UserIdParameter", userId, DbType.Int32);
            }
            if (searchParam.ToLower() != "none") {
                stringParameters += ", @SearchValue=@SearchValueParameter";
                sqlParameters.Add("@SearchValueParameter", searchParam, DbType.String);
            }

            if (stringParameters.Length > 0) {
                sql += stringParameters.Substring(1);
            }
            
            return _dapper.QueryWithParameters<Post>(sql, sqlParameters);
        }

        [HttpGet("GetMyPosts")]
        public IEnumerable<Post> GetMyPosts() {
            // string sql = @"EXEC app.spPost_Get
            //     @UserId = " + this.User.FindFirst("userId")?.Value;
            // this.User is the user from the ControllerBase, not from User.cs
                
            string sql = @"EXEC app.spPost_Get @UserId=@UserIdParameter";
            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);

            return _dapper.QueryWithParameters<Post>(sql, sqlParameters);
        }

        [HttpPut("UpsertPost")]
        public IActionResult UpsertPost(Post postToUpsert) {
            string sql = @"EXEC app.spPost_Upsert
                @UserId=@UserIdParameter, 
                @PostTitle=@PostTitleParameter, 
                @PostContent=@PostContentParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@PostTitleParameter", postToUpsert.PostTitle, DbType.String);
            sqlParameters.Add("@PostContentParameter", postToUpsert.PostContent, DbType.String);

            if (postToUpsert.PostId > 0) {
                sql += ", @PostId=@PostIdParameter";
                sqlParameters.Add("@PostIdParameter", postToUpsert.PostId, DbType.Int32);
            }

            if (_dapper.ExecuteSqlWithDynamicParameters(sql, sqlParameters)) {
                return Ok();
            }

            throw new Exception("Failed to upsert new post.");
        }
        
        [HttpDelete("DeletePost/{postId}")]
        public IActionResult DeletePost(int postId) {
            string sql = @"EXEC app.spPost_Delete 
                @UserId=@UserIdParameter, 
                @PostId=@PostIdParameter";

            DynamicParameters sqlParameters = new DynamicParameters();
            sqlParameters.Add("@UserIdParameter", this.User.FindFirst("userId")?.Value, DbType.Int32);
            sqlParameters.Add("@PostIdParameter", postId, DbType.Int32);
            
            if (_dapper.ExecuteSqlWithDynamicParameters(sql, sqlParameters)) {
                return Ok();
            }

            throw new Exception("Failed to delete post!");
        }
    }
}