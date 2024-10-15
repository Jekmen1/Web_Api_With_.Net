using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
    }
}
