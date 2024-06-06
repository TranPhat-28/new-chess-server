using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace new_chess_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InitializeController : ControllerBase
    {
        [HttpGet("Ready")]
        public ActionResult<ServiceResponse<string>> InitialConnection()
        {
            var response = new ServiceResponse<string>
            {
                Data = "Server is ready"
            };
            return response;
        }
    }
}