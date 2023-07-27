using Microservice.Models;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Microservice.JWT
{
    public class JsonWebToken
    {
        private readonly IConfiguration configuration;


        public JsonWebToken(IConfiguration _configuration)
        {
            configuration = _configuration;

        }
        public string GenerateAccessToken(User getInfoUser, IQueryable<string> getRoles)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);
            List<Claim> claimss = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, getInfoUser.Id.ToString()),
                new Claim(ClaimTypes.Email, getInfoUser.Email.ToString()),
                new Claim(ClaimTypes.Name, getInfoUser.Username.ToString()),
            };
            foreach (var role in getRoles)
            {
                claimss.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var JwtToken = new JwtSecurityToken(
                claims: claimss,
                expires: DateTime.Now.AddSeconds(20),
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                signingCredentials: new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            return token;
        }
        public string GenerateRefreshToken(User getInfoUser, IQueryable<string> getRoles)
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]);
            List<Claim> claimss = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, getInfoUser.Id.ToString()),
                new Claim(ClaimTypes.Name, getInfoUser.Username.ToString()),
            };
            foreach (var role in getRoles)
            {
                claimss.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var JwtToken = new JwtSecurityToken(
                claims: claimss,
                expires: DateTime.Now.AddMinutes(20),
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                signingCredentials: new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
            var RefreshToken = new JwtSecurityTokenHandler().WriteToken(JwtToken);
            return RefreshToken;
        }
    }
}
