//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Abstractions;
//using StoredProcuduresTest.Data;
//using StoredProcuduresTest.Dtos;
//using StoredProcuduresTest.Models;
//namespace StoredProcuduresTest.Controller
//{

//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        DapperContext _dapperContext;

//        public UserController(IConfiguration configuration)
//        {
//            _dapperContext = new DapperContext(configuration);
//        }

//        [HttpGet]
//        public async Task<IActionResult> Get()
//        {
//            string sql = @"
//                SELECT *
//                FROM TutorialAppSchema.Users";
//            var users = await _dapperContext.LoadDataAsync<User>(sql);
//            return Ok(users);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> Get(int id)
//        {
//            var user = await _dapperContext.LoadDataSingleAsync<User>($"SELECT * FROM TutorialAppSchema.Users WHERE [UserId] = {id}");
//            return Ok(user);
//        }

//        [HttpPost]
//        public async Task<IActionResult> PostAsync([FromBody] UserDto user)
//        {
//            string sql = @"INSERT INTO TutorialAppSchema.Users(
//                [FirstName],
//                [LastName],
//                [Email],
//                [Gender],
//                [Active]
//            ) VALUES (" +
//                "'" + user.FirstName +
//                "', '" + user.LastName +
//                "', '" + user.Email +
//                "', '" + user.Gender +
//                "', '" + user.Active +
//            "')";
//            if (await _dapperContext.ExecuteSql(sql))
//                return Ok(user);

//            return BadRequest();
//        }

//        [HttpPut]
//        public async Task<IActionResult> EditUserAsync(User user)
//        {
//            string sql = @"
//        UPDATE TutorialAppSchema.Users
//            SET [FirstName] = '" + user.FirstName +
//                    "', [LastName] = '" + user.LastName +
//                    "', [Email] = '" + user.Email +
//                    "', [Gender] = '" + user.Gender +
//                    "', [Active] = '" + user.Active +
//                "' WHERE UserId = " + user.UserId;

//            Console.WriteLine(sql);

//            if (await _dapperContext.ExecuteSql(sql))
//                return Ok();

//            return BadRequest();
//        }


//        [HttpDelete("{userId}")]
//        public async Task<IActionResult> DeleteUser(int userId)
//        {
//            string sql = @"
//            DELETE * FROM TutorialAppSchema.Users 
//                WHERE UserId = " + userId.ToString();

//            Console.WriteLine(sql);

//            if (await _dapperContext.ExecuteSql(sql))
//                return Ok();

//            return BadRequest();
//        }
//    }
//}
