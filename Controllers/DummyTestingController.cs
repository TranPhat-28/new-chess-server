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

        [HttpGet("Add")]
        public async Task<ActionResult<ServiceResponse<string>>> AddUser1And2AsFriend()
        {
            var response = new ServiceResponse<string>();

            // User 1: Winston
            User? user1 = await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == 1);
            // User 2: James
            User? user2 = await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == 4);

            // if (user1 == null || user2 == null)
            // {
            //     response.Data = "User 1 or 2 is null";
            //     return response;
            // }

            // Winston has James as friend
            user1!.FriendList.Add(user2!);
            
            // user2.FriendList!.Add(user1);
            
            // Else check for duplicates before adding
            // else
            // {
            //     foreach (User friend in friendListA)
            //     {
            //         int index = ownedWeapons.FindIndex(item => item.Id == obtainedWeapon.Id);
            //         if (index >= 0)
            //         {
            //             // Element exists then skip
            //             continue;
            //         }
            //         else
            //         {
            //             ownedWeapons.Add(obtainedWeapon);
            //         }
            //     }
            // }

            // Save changes
            await _dataContext.SaveChangesAsync();
            // Add 1 as 2 friend

            response.Data = "User 1 and 2 has been added as friend";

            return response;
        }
    }
}