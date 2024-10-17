using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DotNetApi.Data;
using DotNetApi.Models;
using DotNetApi.Dtos;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserEFController : ControllerBase
    {
        DataContextEF _entityFramework;

        IUserRepository _userRepository;

        IMapper _mapper;

        public UserEFController(IConfiguration config, IUserRepository _userRepository)
        {
            _entityFramework = new DataContextEF(config);

            _userRepository = user;

            _mapper = new Mapper(new MapperConfiguration(cfg => {
                cfg.CreateMap<UserAddDto, User>();
                cfg.CreateMap<UserSalary, UserSalary>();
                cfg.CreateMap<UserJobInfo, UserJobInfo>();
                
            }));
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>();
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            User? user = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault<User>();

            if (user != null)
            {
                return user;
            }
            throw new Exception("Failed to get user");
        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _entityFramework.Users
                .Where(u => u.UserId == user.UserId)
                .FirstOrDefault<User>();

            if (userDb != null)
            {
                userDb.Active = user.Active;
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                userDb.Email = user.Email; 
                userDb.Gender = user.Gender;

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Failed to update user");
            }
            return NotFound();
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(UserAddDto user)
        {
            User userDb = _mapper.Map<User>(user);
            
            

            _userRepository.Users.AddEntity<User>(userDb);

            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to add user");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            User? userDb = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault<User>();

            if (userDb != null)
            {
                _userRepository.RemoveEntity<User>(userDb);

                if (_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("Failed to delete user");
            }
            return NotFound();
        }

        [HttpGet("UserSalary/{userId}")]
        public IEnumerable<UserSalary> GetUserSalaryEF(int userId)
        {
            return _entityFramework.UserSalary
                .Where(U => u.UserId == userId)
                .ToList();
        }

        [HttpPost("UserSalary")]
        public IActionResult PostUserSalaryEf(UserSalary userForInsert)
        {
            _userRepository.AddEntity<UserSalary>(yuserForInsert)
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Adding UserSalary failed on save")

        }

        [HttpPut("UserSalary")]
        public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
        {
            UserSalary? userToUpdate = _entityFramework.UserSalary
                .Where(u => u.UserId == userForUpdate.UserId)
                .FirstOrDefault(); 

            if(userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if(_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("fail to update")
            }



        }

        [HttpDelete("UserSalary/{userId}")]
        public IActionResult DeleteUserSalaryEf(int userId)
        {
            UserSalary? userToDelete = _entityFramework.UserSalary
                .Where(u => u.UserId == userId)
                .FirstOrDefault();

            if (userToDelete != null)
            {
                _userRepository.RemoveEntity<UserSalary>(userToDelete);
                if(_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("error");
            }
        }

        [HttpGet("UserJobInfo/{userId}")]
        public IEnumerable<UserJobInfo> GetUserJobInfoEF(int userId)
        {
            return _entityFrameworkGetUserJobInfo
                .Where(U => u.UserId == userId)
                .ToList();
        }

        [HttpPost("UserJobInfo")]
        public IActionResult PostUserUserJobInfoEf(UserUserJobInfo userForInsert)
        {
            _userRepository.AddEntity<UserJobInfo>(userForInsert)
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Adding UserSalary failed on save")

        }


        [HttpPut("UserJobInfo")]
        public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
        {
            UserJobInfo? userToUpdate = _entityFramework.UserJobInfo
                .Where(u => u.UserId == userForUpdate.UserId)
                .FirstOrDefault(); 

            if(userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if(_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("fail to update")
            }

        }

        [HttpDelete("UserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfoEf(int userId)
        {
            UserJobInfo? userToDelete = _entityFramework.UserJobInfo
                .Where(u => u.UserId == userId)
                .FirstOrDefault();

            if (userToDelete != null)
            {
                _userRepository.RemoveEntity<UserJobInfo>(userToDelete);
                if(_userRepository.SaveChanges())
                {
                    return Ok();
                }
                throw new Exception("error");
            }
        }
    }
}