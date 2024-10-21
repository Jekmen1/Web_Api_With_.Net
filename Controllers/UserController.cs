using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DotNetApi.Data;
using DotNetApi.Models;
using DotNetApi.Dtos;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            string sql = @"
            SELECT [UserId], [FirstName], [LastName], [Email], [Gender], [Active]
            FROM TutorialAppSchema.Users";
            return _dapper.LoadData<User>(sql);
        }

        [HttpGet("GetSingleUser/{userId}")]
        public ActionResult<User> GetSingleUser(int userId)
        {
            string sql = @"
            SELECT [UserId], [FirstName], [LastName], [Email], [Gender], [Active]
            FROM TutorialAppSchema.Users
            WHERE UserId = @UserId";
            var user = _dapper.LoadDataSingle<User>(sql, new { UserId = userId });
            return user != null ? Ok(user) : NotFound("User not found");
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            string sql = @"
            UPDATE TutorialAppSchema.Users
            SET [FirstName] = @FirstName, [LastName] = @LastName, [Email] = @Email, [Gender] = @Gender, [Active] = @Active
            WHERE UserId = @UserId";
            
            var parameters = new 
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Gender,
                user.Active,
                user.UserId
            };

            if (_dapper.ExecuteSqlWithParameters(sql, parameters))
                return Ok();
            
            return BadRequest("Failed to update user");
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserAddDto user)
        {
            string sql = @"
            INSERT INTO TutorialAppSchema.Users (FirstName, LastName, Email, Gender, Active)
            VALUES (@FirstName, @LastName, @Email, @Gender, @Active)";
            
            var parameters = new 
            {
                user.FirstName,
                user.LastName,
                user.Email,
                user.Gender,
                user.Active
            };

            if (_dapper.ExecuteSqlWithParameters(sql, parameters))
                return Ok();
            
            return BadRequest("Failed to add user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            string sql = "DELETE FROM TutorialAppSchema.Users WHERE UserId = @UserId";
            if (_dapper.ExecuteSqlWithParameters(sql, new { UserId = userId }))
                return Ok();
            
            return BadRequest("Failed to delete user");
        }
    }
}
