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

        public DatabaseTestingController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("/CreateMock")]
        public async Task<ActionResult<string>> ConnectionTesting()
        {
            User mockUser = new User()
            {
                Email = "mock@mail.com",
                Name = "Mock"
            };

            _dataContext.Users.Add(mockUser);
            await _dataContext.SaveChangesAsync();

            return "OK";
        }
    }
}