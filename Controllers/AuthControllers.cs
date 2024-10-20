using DotNetApi.Dtos;
using System.Security.Cryptograpy;
using System.Text;

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
        public IActionResult Register(UserForRegistrationDto UserForRegistration)
        {
            if(UserForRegistration.Password == UserForRegistration.PasswordConfrim)
            {
                string sqlCheckUserExists = "SELECT * FROM TutorialApp.Schema.Auth WHERE Email = '" + 
                    userForRegistration.Email + "'";

                IEnumerable<string> exsisingUsers = _dapper.LoadData<string>(sql);
                if(exsisingUsers.Count() == 0)
                {
                    byte [] passwordSalt = new byte[128 /8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    string passwordSaltPlusString = _config.GetSection("AppSettings:PasswordKey").Value + 
                        Convert.ToBase64String(passwordSalt);

                    byte[] asswordHash = KeyDerivation.Pbkdf2
                    (
                        Password: userForRegistration.Password,
                        salt: Encoding.ASCII.GetBytes(passwordSaltPlusString),
                        prf: KeyDerivationPrf.HMACSHA245,
                        iterationCount: 100000,
                        numbyBytesRequested: 256/ 8

                    );

                    string sqlAddAuth = @"
                        INSERT INTO TutorialAppSchema.Auth ([Email],
                        [PasswordHash],
                        [PasswordSalt]) VALUES (" + userForRegistration.Email + 
                        "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameter = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;   

                    sqlParameter.Add(passwordSaltParameter);
                    sqlParameter.Add(passwordHashParameter);  

                    if(_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        return Ok();
                    }               

                    trow new Exception("failed to register user.");
                }
                trow new Exception("User with this email already exsist");
            }

            trow new Exception("Password do not match");
        }

        [HttpPost("Login")]  
        public IActionResult Login(UserForLoginDto UserForLogin)
        {
            return Ok();
        }  
    }
}