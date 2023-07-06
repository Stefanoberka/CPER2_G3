using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CPER2G3.Earth4Sport.AzureFunction.JwtUtils {
    public static class JwtMethods {
        private readonly static string _mySecret = "Vmware1!Vmware2?Vmware3;";
        private readonly static SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_mySecret));
        private readonly static string _myIssuer = "https://www.cper2g3.com";
        private readonly static string _myAudience = "https://www.earth4sport.com";
        public static string GenerateToken(string userId) {


            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsIdentity = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,userId),
                });
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _myIssuer,
                Audience = _myAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool ValidateCurrentToken(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            try {
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _myIssuer,
                    ValidAudience = _myAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch {
                return false;
            }
            return true;
        }

        public static string GetTokenUserId(string token) {
            var handler = new JwtSecurityTokenHandler();
            var content = handler.ReadJwtToken(token);
            var nameid = content.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            return nameid;
        }

    }
}
