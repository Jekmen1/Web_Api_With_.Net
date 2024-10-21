using DotNetApi.Dtos;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using DotNetApi.Models;
using DotNetApi.Data;

namespace DotnetApi.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
        }

        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password != userForRegistration.PasswordConfirm)
                return BadRequest("Passwords do not match");

            string sqlCheckUserExists = "SELECT COUNT(1) FROM TutorialAppSchema.Auth WHERE Email = @Email";
            int userExists = _dapper.LoadDataSingle<int>(sqlCheckUserExists, new { userForRegistration.Email });

            if (userExists > 0)
                return BadRequest("User with this email already exists.");

            byte[] passwordSalt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(passwordSalt);

            byte[] passwordHash = GetPasswordHash(userForRegistration.Password, passwordSalt);

            string sqlAddAuth = @"
                INSERT INTO TutorialAppSchema.Auth (Email, PasswordHash, PasswordSalt)
                VALUES (@Email, @PasswordHash, @PasswordSalt)";
            
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Email", userForRegistration.Email),
                new SqlParameter("@PasswordHash", SqlDbType.VarBinary) { Value = passwordHash },
                new SqlParameter("@PasswordSalt", SqlDbType.VarBinary) { Value = passwordSalt }
            };

            if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                return Ok();
            
            return BadRequest("Failed to register user.");
        }

        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            string sqlForHashAndSalt = @"
                SELECT PasswordHash, PasswordSalt FROM TutorialAppSchema.Auth WHERE Email = @Email";

            var userForConfirmation = _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt, new { userForLogin.Email });
            if (userForConfirmation == null)
                return Unauthorized("Invalid credentials");

            byte[] passwordHash = GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

            if (!passwordHash.SequenceEqual(userForConfirmation.PasswordHash))
                return Unauthorized("Incorrect password");

            return Ok();
        }

        private byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            );
        }
    }
}
