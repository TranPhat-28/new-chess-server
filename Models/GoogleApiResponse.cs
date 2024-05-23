using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using new_chess_server.Models.Enums;

namespace new_chess_server.Models
{
    public class GoogleApiResponse
    {
        // Valid token will return these
        public string Iss { get; set; } = "";
        public string Azp { get; set; } = "";
        public string Aud { get; set; } = "";
        public string Sub { get; set; } = "";
        public string Email { get; set; } = "";
        public string Email_verified { get; set; } = "";
        public string Nbf { get; set; } = "";
        public string Name { get; set; } = "";
        public string Picture { get; set; } = "";
        public string Given_name { get; set; } = "";
        public string Family_name { get; set; } = "";
        public string Iat { get; set; } = "";
        public string Exp { get; set; } = "";
        public string Jti { get; set; } = "";
        public string Alg { get; set; } = "";
        public string Kid { get; set; } = "";
        public string Typ { get; set; } = "";
    }

    public class GoogleApiErrorResponse
    {
        // Invalid token will return these
        public string Error { get; set; } = "";
        public string Error_description { get; set; } = "";
    }

    public class GetGoogleTokenVerification
    {
        public GoogleApiResponseType ResponseType { get; set; }
        public string Value { get; set; } = "";
    }
}