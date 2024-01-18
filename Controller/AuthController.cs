using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using StoredProcuduresTest.Data;
using StoredProcuduresTest.Dtos;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using StoredProcuduresTest.Helpers;
using Dapper;
using System.Data;
using System.Runtime.Intrinsics.Arm;

namespace StoredProcuduresTest.Controller
{

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IDapperContext _dapper;
        private readonly IAuthHelper _authHelper;

        public AuthController(IConfiguration config, IDapperContext dp, IAuthHelper ah)
        {
            _authHelper = ah;
            _dapper = dp;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" +
                    userForRegistration.Email + "'";

                IEnumerable<string> existingUsers = await _dapper.LoadDataAsync<string>(sqlCheckUserExists);
                if (existingUsers.Count() == 0)
                {
                    UserForLogin userForSetPassword = new UserForLogin()
                    {
                        Email = userForRegistration.Email,
                        Password = userForRegistration.Password
                    };
                    if (await _authHelper.SetPasswordAsync(userForSetPassword))
                    {

                        string sqlAddUser = @"EXEC TutorialAppSchema.spUser_Upsert
                            @FirstName = '" + userForRegistration.FirstName +
                            "', @LastName = '" + userForRegistration.LastName +
                            "', @Email = '" + userForRegistration.Email +
                            "', @Gender = '" + userForRegistration.Gender +
                            "', @Active = 1" +
                            ", @JobTitle = '" + userForRegistration.JobTitle +
                            "', @Department = '" + userForRegistration.Department +
                            "', @Salary = '" + userForRegistration.Salary + "'";
         
                        if (await _dapper.ExecuteSql(sqlAddUser))
                        {
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
        public async Task<IActionResult> ResetPasswordAsync(UserForLogin userForSetPassword)
        {
            if (await _authHelper.SetPasswordAsync(userForSetPassword))
            {
                return Ok();
            }
            throw new Exception("Failed to update password!");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(UserForLogin userForLogin)
        {
            string sqlForHashAndSalt = @"EXEC TutorialAppSchema.spLoginConfirmation_Get 
                @Email = @EmailParam";

            DynamicParameters sqlParameters = new DynamicParameters();

            // SqlParameter emailParameter = new SqlParameter("@EmailParam", SqlDbType.VarChar);
            // emailParameter.Value = userForLogin.Email;
            // sqlParameters.Add(emailParameter);

            sqlParameters.Add("@EmailParam", userForLogin.Email, DbType.String);

            UserLoginConfirmation userForConfirmation = _dapper
                .LoadDataSingleWithParameters<UserLoginConfirmation>(sqlForHashAndSalt, sqlParameters);

            byte[] passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

            // if (passwordHash == userForConfirmation.PasswordHash) // Won't work

            for (int index = 0; index < passwordHash.Length; index++)
            {
                if (passwordHash[index] != userForConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Incorrect password!");
                }
            }

            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" +
                userForLogin.Email + "'";

            int userId = await _dapper.LoadDataSingleAsync<int>(userIdSql);

            return Ok(new Dictionary<string, string> {
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        [HttpGet("RefreshToken")]
        public async Task<string> RefreshToken()
        {
            string userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" +
                User.FindFirst("userId")?.Value + "'";

            int userId = await _dapper.LoadDataSingleAsync<int>(userIdSql);

            return _authHelper.CreateToken(userId);
        }

    }
}
