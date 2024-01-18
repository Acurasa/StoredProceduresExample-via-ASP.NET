using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using StoredProcuduresTest.Data;
using StoredProcuduresTest.Dtos;
using StoredProcuduresTest.Models;

namespace StoredProcuduresTest.Controller
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class FragmentController : ControllerBase
    {
        private readonly IDapperContext _dapper;
        public FragmentController(IConfiguration config, IDapperContext dp)
        {
            _dapper = dp;
        }

        [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
        public async Task<IEnumerable<Fragment>> GetPostsAsync(int postId = 0, int userId = 0, string searchParam = "None")
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get";
            string parameters = "";

            if (postId != 0)
            {
                parameters += ", @PostId=" + postId.ToString();
            }
            if (userId != 0)
            {
                parameters += ", @UserId=" + userId.ToString();
            }
            if (searchParam.ToLower() != "none")
            {
                parameters += ", @SearchValue='" + searchParam + "'";
            }

            if (parameters.Length > 0)
            {
                sql += parameters.Substring(1);
            }

            return await _dapper.LoadDataAsync<Fragment>(sql);
        }

        [HttpGet("MyPosts")]
        public async Task<IEnumerable<Fragment>> GetMyPosts()
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get @UserId = " +
                this.User.FindFirst("userId")?.Value;

            return await _dapper.LoadDataAsync<Fragment>(sql);
        }

        [HttpPut("UpsertPost")]
        public async Task<IActionResult> UpsertPostAsync(Fragment postToUpsert)
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Upsert
                @UserId =" + this.User.FindFirst("userId")?.Value +
                ", @PostTitle ='" + postToUpsert.PostTitle +
                "', @PostContent ='" + postToUpsert.PostContent + "'";

            if (postToUpsert.PostId > 0)
            {
                sql += ", @PostId = " + postToUpsert.PostId;
            }

            if (await _dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to upsert post!");
        }


        [HttpDelete("Post/{postId}")]
        public async Task<IActionResult> DeletePostAsync(int postId)
        {
            string sql = @"EXEC TutorialAppSchema.spPost_Delete @PostId = " +
                    postId.ToString() +
                    ", @UserId = " + this.User.FindFirst("userId")?.Value;


            if (await _dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete post!");
        }
    }
}
