using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<ServiceResponse<string>> LoginWithGoogle(AuthenticationPostDto authenticationPostDto);
    }
}