using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using new_chess_server.Data;

namespace new_chess_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseTestingController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public DatabaseTestingController(DataContext dataContext, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _configuration = configuration;
        }

        [HttpGet("CreateMock")]
        public async Task<ActionResult<string>> ConnectionTesting()
        {
            User mockUser = new User()
            {
                ExternalID = "114299150669811858083",
                Email = "jamesjohnjj89104@gmail.com",
                Name = "James John",
                Picture = "https://lh3.googleusercontent.com/a/ACg8ocKLj4uYpLkq7HcbssTy1QFP6R5xgS3AaJ9kM7DIAceONCex-wQ=s96-c"
            };

            _dataContext.Users.Add(mockUser);
            await _dataContext.SaveChangesAsync();

            return "OK";
        }

        [HttpGet("UserSecret")]
        public string ReadFromUserSecrets()
        {
            var secret = _configuration.GetSection("JWT:Token").Value;
            Console.WriteLine(secret);
            return "Ok";
        }
    }
}