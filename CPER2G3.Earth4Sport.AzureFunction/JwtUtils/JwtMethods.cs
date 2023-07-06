using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CPER2G3.Earth4Sport.AzureFunction.JwtUtils
{
    public class JwtMethods
    {
        private string _JwtSecret { get; set; }
        private SymmetricSecurityKey _JwtSecurityKey { get; set; }
        private readonly string _Issuer = "https://www.cper2g3.com";
        private readonly string _Audience = "https://www.earth4sport.com";

        public JwtMethods(IConfiguration conf)
        {
            _JwtSecret = conf.GetSection("JwtSecret").Value;
            _JwtSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_JwtSecret));
        }
        public string GenerateToken(string userId)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsIdentity = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,userId),
                });
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _Issuer,
                Audience = _Audience,
                SigningCredentials = new SigningCredentials(_JwtSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateCurrentToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _Issuer,
                    ValidAudience = _Audience,
                    IssuerSigningKey = _JwtSecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetTokenUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var content = handler.ReadJwtToken(token);
            var nameid = content.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            return nameid;
        }
    }
}