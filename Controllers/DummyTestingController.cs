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

        [HttpGet("SendFriendRequest")]
        public async Task<ActionResult<ServiceResponse<string>>> AddUser1And2AsFriend()
        {
            var response = new ServiceResponse<string>();

            // User5 send friend request to user4

            // User 1: Winston
            User? user5 = await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == 5);
            // User 2: James
            User? user4 = await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == 4);

            var newRequest = new FriendRequest
            {
                SenderId = 5,
                ReceiverId = 4,
            };

            _dataContext.FriendRequests.Add(newRequest);

            // Save changes
            await _dataContext.SaveChangesAsync();
            // Add 1 as 2 friend

            response.Data = "User5 has sent a friend request to user4";

            return response;
        }
    }
}