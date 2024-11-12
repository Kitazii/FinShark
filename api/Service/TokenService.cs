using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]?? "Is Empty")); //encrypts key in a unique way only specific to our server. So people cannot mess with the integrity of the token.
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? "Is Empty"),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName ?? "Is Empty")
            }; //created are claims

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); //form of encryption

            //.net creates token from object
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //this is the wallet
                Expires = DateTime.Now.AddDays(7), //for security purposes, dont want anyone getting there hands on it
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor); //creates the token

            return tokenHandler.WriteToken(token); //returns token as data type string
        }
    }
}