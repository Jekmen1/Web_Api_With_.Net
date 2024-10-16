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
        
        [HttpGet("UserSalary/{userId}")]
        public IEnumerable<UserSalary> GetSingleUser(int userId)
        {
            return _dapper.LoadData<UserSalary>(@"
            SELECT UserSalary.UserId
                , UserSalary.Salary
            FROM TutorialAppSchema.UserSalary
                WHERE UserId = " + userId);
        }

        [HttpPost("UserSalary")]
        public IActionResult PostUserSalary(UserSalary userSalaryForInsert)
        {
            string sql = @"
                INSERT INFO TutorialAppSchema.UserSalary (
                    UserId,
                    Salary
                ) Values (" + userSalaryForInsert.UserId
                    +", " + userSalaryForInsert.Salary
                    + ")";
            
            if (_dapper.ExecuteSqlWithRowCount(sql) > 0)
            {
                return Ok(userSalaryForInsert);
            }

            throw new Exception("Failed to add usersalary");
        }

        [HttpPut("UserSalary")]
        public IActionResult PutUserSalary(UserSalary userSalaryForUpdate)
        {
            string sql = "UPDATE TutorialAppSchema.UserSalary SET Salary"
            + userSalaryForUpdate.Salary
            + " WHERE UserId=" + userSalaryForUpdate.UserId

            if (_dapper.ExecuteSql(sql))
            {
                return Ok(userSalaryForUpdate);
            }

            throw new Exception("Failed to update usersalary");
        }

        [HttpDelete("UserSalary/{userId}")]
        public IActionResult DeleteUserSalary(int userId)
        {
            string sql = @"DELETE FROM TutorialAppSchema.UserSalary WHETE UserId=" + userId;
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete usersalary");
        }

        [HttpGet("UserJobInfo/{userId}")]
        public IEnumerable<UserJobInfo> GetSingleUser(int userId)
        {
            return _dapper.LoadData<UserJobInfo>(@"
            SELECT UserJobInfo.UserId
                , UserJobInfo.Title
                , UserJobInfo.Department
            FROM TutorialAppSchema.UserJobInfo
                WHERE UserId = " + userId);
        }

        [HttpPost("UserJobInfo")]
        public IActionResult PostUserSalary(UserJobInfo userJobInfoForInsert)
        {
            string sql = @"
                INSERT INFO TutorialAppSchema.UserJobInfo (
                    UserId,
                    title,
                    artment
                ) Values (" + userJobInfoForInsert.UserId
                    +", " + userJobInfoForInsert.Title
                    +", " + userJobInfoForInsert.Department
                    + ")";
            
            if (_dapper.ExecuteSql(sql))
            {
                return Ok(userJobInfoForInsert);
            }

            throw new Exception("Failed to add jobinfo");
        }

        [HttpPut("UserJobInfo")]
        public IActionResult PutUserSalary(UserJobInfo userJobInfoForUpdate)
        {
            string sql = "UPDATE TutorialAppSchema.UserJobInfo SET Department'"
            + userJobInfoForUpdate.Department
            + "', JobTitle = '"
            + userJobInfoForUpdate.JobTitle
            + " WHERE UserId=" + userJobInfoForUpdate.UserId

            if (_dapper.ExecuteSql(sql))
            {
                return Ok(userSalaryForUpdate);
            }

            throw new Exception("Failed to update userupdate");

        }

        [HttpDelete("UserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfo(int userId)
        {
            string sql = @"DELETE FROM TutorialAppSchema.UserJobInfo WHETE UserId=" + userId.ToString();
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }

            throw new Exception("Failed to delete userJobInfo");
        }
    }
}