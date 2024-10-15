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
        DataContextDapper _dapper;

        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("testConnection")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("test/{testValue}")]
        public String[] Test(string testValue)
        {
            string[] responseArray = new String[]
            {
                "test1",
                "test2",
                testValue
            };

            return responseArray;
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            FROM TutorialAppSchema.Users";
            IEnumerable<User> users = _dapper.LoadData<User>(sql);
            return users;
        }
       

        [HttpGet("GetSingleUsers/{userId}")]
        public User GetSingleUsers(int userId)
        {
            
            string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            FROM TutorialAppSchema.Users
                WHERE UserId = " + userId.ToString();
            User users = _dapper.LoadDataSingle<User>(sql);
            return users;
        }
    

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            string sql = @"
            UPDATE TutorialAppSchema.Users
                SET [FirstName] = '" + user.FirstName + 
                    "',[LastName] = '" + user.LastName + 
                    "',[Email] = '" + user.Email + 
                    "',[Gender] = '" + user.Gender + 
                    "',[Active] = '" + user.Active +
                "' Where UserId = " + user.UserId;
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to update user");
        }


        [HttpPost("AddUser")]
        public IActionResult AddUser(UserAddDto user)
        {
            string sql = @"INSERT INTO TutorialAppSchema.Users(
                    [FirstName],
                    [LastName],
                    [Email],
                    [Gender],
                    [Active]
                ) VALUES (" +
                    "'" + user.FirstName + 
                    "', '" + user.LastName + 
                    "', '" + user.Email + 
                    "', '" + user.Gender + 
                    "', '" + user.Active +
                "')";
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to add user");
            
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            string sql = @"
                DELETE FROM TutorialAppSchema.Users    
                    WHEREUserId = " + userId.ToString();
                    
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to remove user");
        }
        
    
    }
}