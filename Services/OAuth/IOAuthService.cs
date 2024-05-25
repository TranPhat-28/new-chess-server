using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace new_chess_server.Services.OAuth
{
    public interface IOAuthService
    {
        Task<ServiceResponse<GoogleApiResponse>> VerifyGoogleIDToken(string googleIDToken);
    }
}