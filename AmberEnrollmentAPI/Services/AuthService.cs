using AmberEnrollmentAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AmberEnrollmentAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration config;


        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }



        //register
        public async Task<bool> RegisterUser(User user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Username,
                Email = user.Username,
            };

            var result = await userManager.CreateAsync(identityUser, user.Password);

            //Setup Role Based code
            if (result.Succeeded)
            {
                var adminRole = "Admin"; // Role name
                await userManager.AddToRoleAsync(identityUser, adminRole);
            }
            return result.Succeeded;
        }


        //login
        public async Task<bool> Login(User user)
        {
            var identityUser = await userManager.FindByEmailAsync(user.Username);
            if (identityUser == null)
            {
                return false;
            }
            return await userManager.CheckPasswordAsync(identityUser, user.Password);
        }


       /// set up AuthAPI Controller First to generate token from their.
       
        public string GenerateToken(User user)
        {
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Username), // Username is same as Email
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWTConfig:Key").Value));
            var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
            var securityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    issuer: config.GetSection("JWTConfig: Issuer").Value,
                    audience: config.GetSection("JWTConfig: Audience").Value,
                    signingCredentials: signingCreds
                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}



//
//Extract IAuth interface
// Set up Auth API Controller
// Genrate Token from Auth API Controller