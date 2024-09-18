using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using new_chess_server.Data;

namespace new_chess_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DummyTestingController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public DummyTestingController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // [HttpGet("Dummy")]
        // public async Task<ActionResult<ServiceResponse<string>>> Dummy()
        // {
        //     var response = new ServiceResponse<string>();

        //     // User 1: Winston
        //     User? user1 = await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == 1);
        //     // User 2: James
        //     User? user2 = await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == 2);

        //     user1!.FriendList.Remove(user2!);
        //     user2!.FriendList.Remove(user1!);

        //     // Save changes
        //     await _dataContext.SaveChangesAsync();

        //     response.Data = "OK";

        //     return response;
        // }
    }
}