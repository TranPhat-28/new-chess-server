using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Json;
using new_chess_server.Services.OAuth;
using new_chess_server.Data;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace new_chess_server.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IOAuthService _oAuthService;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IOAuthService oAuthService, DataContext dataContext, IConfiguration configuration)
        {
            _oAuthService = oAuthService;
            _dataContext = dataContext;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<string>> LoginWithGoogle(AuthenticationPostDto authenticationPostDto)
        {
            // Response
            var response = new ServiceResponse<string>();

            // Verify the token
            var result = await _oAuthService.VerifyGoogleIDToken(authenticationPostDto.Token);

            // Token is valid, perform register or login accordingly
            if (result.IsSuccess)
            {
                // Check if user is in db
                var user = await _dataContext.Users.FirstOrDefaultAsync(user => user.Email == result.Data!.Email && user.ExternalID == result.Data!.Sub);

                // If already in db then return JWT
                if (user is not null)
                {
                    var newJWT = CreateJWTToken(user);
                    response.Data = newJWT;
                }
                // If not in db then Register
                else
                {
                    // Register by adding user to db
                    User newUser = new User()
                    {
                        Email = result.Data!.Email,
                        Name = result.Data!.Name,
                        ExternalID = result.Data!.Sub,
                        Picture = result.Data!.Picture.Replace("s96-c", "s192-c")
                    };

                    // Create a new GameStatistic instance
                    GameStatistic newStatistic = new GameStatistic()
                    {
                        // Establish the relationship to the User
                        User = newUser
                    };

                    // Establish the relationship from User to the GameStatistic
                    newUser.Statistic = newStatistic;

                    _dataContext.Users.Add(newUser);
                    _dataContext.GameStatistics.Add(newStatistic);

                    await _dataContext.SaveChangesAsync();

                    // Then create and return a JWT
                    var newJWT = CreateJWTToken(newUser);
                    response.Data = newJWT;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "We could not verify you with the provider.";
            }

            return response;
        }

        private string CreateJWTToken(User user)
        {
            // The list of Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            // Getting the secret from User Secrets

            // Use GetSection for local development
            // Use Environment.GetEnvironmentVariable for production

            // -----------CHANGE FOR DEPLOYMENT----------------
            // var secretToken = _configuration.GetSection("JWT:Token").Value;
            var secretToken = Environment.GetEnvironmentVariable("JWTSecretString");

            // Check if token is null
            if (secretToken is null)
            {
                throw new Exception("AppSettings token is null");
            }

            // Symmetric key for the token with secret is the AppSettings token
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretToken));

            // For signing the token
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Storing some information such as Claims and Expiring day for the final token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // JWT handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            // Use the handler to create the token with the tokenDescriptor
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            // Write the token
            return tokenHandler.WriteToken(token);
        }
    }
}