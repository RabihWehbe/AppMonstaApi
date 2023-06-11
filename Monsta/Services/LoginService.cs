using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Monsta.Data;
using Monsta.dto;
using Monsta.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Monsta.Services
{
    public class LoginService
    {
        AppDbContext _context { get; set; }

        private readonly IConfiguration _config;


        //IConfiguration instance to get the token from appStetings
        public LoginService(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        public async Task<LoginDto> verify(UserDto user)
        {
            var userRef = await _context.users
                .Where(r => r.UserEmail == user.UserEmail)
                .Select(u => new User
                {
                    UserId = u.UserId,
                    UserEmail = u.UserEmail,
                    UserName = u.UserName,
                    PasswordHash = u.PasswordHash,
                    PasswordSalt = u.PasswordSalt,
                }).FirstOrDefaultAsync();

            if(userRef != null)
            {
                if (!VerifyPasswordHash(user.Password, userRef.PasswordHash, userRef.PasswordSalt))
                {
                    return new LoginDto();
                }

                string tokenvalue = CreateToken(userRef);

                return new LoginDto
                {
                    token = tokenvalue
                };
            }

            return null;
        }




        private string CreateToken(User user)
        {
            //claims are prprties describe the user that is authenticated
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.UserEmail)
            };

            //we need a key inside encoding, usually we put it in appSettings.json
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
             _config.GetSection("Settings:Token").Value));

            //after creating our key we need signing credentials
            var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }



        private bool VerifyPasswordHash(string password, byte[] passwordHash,byte[] passwordSalt)
        {
            var hmac = new HMACSHA512(passwordSalt);

            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);

        }
    }
}
