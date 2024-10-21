using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DotNetApi.Data;
using DotNetApi.Models;
using DotNetApi.Dtos;
using AutoMapper;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserEFController : ControllerBase
    {

        IUserRepository _userRepository;

        IMapper _mapper;

        public UserEFController(IConfiguration config, IUserRepository _userRepository)
        {

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
            IEnumerable<User> users = _userRepository.GetUsers();
            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {

            return _userRepository.GetSingleUser(userId);

        }

        [HttpPut("EditUser")]
        public IActionResult EditUser(User user)
        {
            User? userDb = _userRepository.GetSingleUser(user.UserId);


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
            User? userDb = _userRepository.GetSingleUser(user.UserId);

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
        public IEnumerable<decimal> GetUserSalaryEF(int userId)
        {
            return _userRepository.GetSingleUserSalary(userId);
        }

        [HttpPost("UserSalary")]
        public IActionResult PostUserSalaryEf(UserSalary userForInsert)
        {
            _userRepository.AddEntity<UserSalary>(yuserForInsert);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Adding UserSalary failed on save");

        }

        [HttpPut("UserSalary")]
        public IActionResult PutUserSalaryEf(UserSalary userForUpdate)
        {
            UserSalary? userToUpdate = _userRepository.GetSingleUserSalary(userForUpdate.UserId); 

            if(userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if(_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("fail to update");
            }



        }

        [HttpDelete("UserSalary/{userId}")]
        public IActionResult DeleteUserSalaryEf(int userId)
        {
            UserSalary? userToDelete = _userRepository.GetSingleUserSalary(userId);

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
        public UserJobInfo GetUserJobInfoEF(int userId)
        {
            return _userRepository.GetSingleUserJobInfo(userId);
        }

        [HttpPost("UserJobInfo")]
        public IActionResult PostUserJobInfoEf(UserJobInfo userForInsert)
        {
            _userRepository.AddEntity<UserJobInfo>(userForInsert);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Adding UserSalary failed on save");

        }


        [HttpPut("UserJobInfo")]
        public IActionResult PutUserJobInfoEf(UserJobInfo userForUpdate)
        {
            UserJobInfo? userToUpdate = _userRepository.GetSingleUserJobInfo(userForUpdate.UserId); 

            if(userToUpdate != null)
            {
                _mapper.Map(userForUpdate, userToUpdate);
                if(_userRepository.SaveChanges())
                {
                    return Ok();
                }

                throw new Exception("fail to update");
            }

        }

        [HttpDelete("UserJobInfo/{userId}")]
        public IActionResult DeleteUserJobInfoEf(int userId)
        {
            UserJobInfo? userToDelete = _userRepository.GetSingleUserJobInfo(userId);

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