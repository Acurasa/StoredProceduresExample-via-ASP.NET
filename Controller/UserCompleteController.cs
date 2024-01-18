using Microsoft.AspNetCore.Mvc;
using StoredProcuduresTest.Data;
using StoredProcuduresTest.Dtos;
using StoredProcuduresTest.Models;

namespace StoredProcuduresTest.Controller
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserCompleteController : ControllerBase
    {
        IDapperContext _dapper;
        public UserCompleteController(IConfiguration config, IDapperContext dp)
        {
            _dapper = dp;
        }

        [HttpGet("TestConnection")]
        public async Task<DateTime> TestConnection()
        {
            return await _dapper.LoadDataSingleAsync<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("GetUsers/{userId}/{isActive}")]
        // public IEnumerable<User> GetUsers()
        public async Task<IEnumerable<UserComplete>> GetUsers(int userId, bool isActive)
        {
            string sql = @"EXEC TutorialAppSchema.spUsers_Get";
            string parameters = "";

            if (userId != 0)
            {
                parameters += ", @UserId=" + userId.ToString();
            }
            if (isActive)
            {
                parameters += ", @Active=" + isActive.ToString();
            }

            if (parameters.Length > 0)
            {
                sql += parameters.Substring(1);//, parameters.Length);
            }

            IEnumerable<UserComplete> users = await _dapper.LoadDataAsync<UserComplete>(sql);
            return users;
        }

        [HttpPut]
        public async Task<IActionResult> Task<UpsertUser>(UserComplete user)
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

            if (await _dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Update User");
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> TaskAsync<DeleteUser>(int userId)
        {
            string sql = @"TutorialAppSchema.spUser_Delete
            @UserId = " + userId.ToString();

            if (await _dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to Delete User");
        }
    }
}
