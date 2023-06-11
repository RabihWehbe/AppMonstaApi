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
    public class RegisterService
    {
        AppDbContext _context { get; set; }

        private readonly IConfiguration _config;

        public RegisterService(AppDbContext context,IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }


        
        public async Task<RegisterDto> register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.UserName = request.UserName;
            user.UserEmail = request.UserEmail;

            string token = CreateToken(user);

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return new RegisterDto
            {
                user = request,
                token = token
            };
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
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            var hmac = new HMACSHA512();

            passwordSalt = hmac.Key;

            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
