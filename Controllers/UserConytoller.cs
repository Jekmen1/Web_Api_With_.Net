using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {
             
        }


        [HttpGet("test/{testValue}")]
        // public IActionResult Test()
        
        public String[] Test(string testValue)
        {
            string[] responseArray = new String[] {
                "test1",
                "test2",
                testValue
            };

            return responseArray;
        }
    }


}
