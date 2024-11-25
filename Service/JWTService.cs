using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GerenciamentoDeEndereco.Service
{
    public class JwtService
    {
        private readonly string _key;

        /// <summary>
        /// Constructor that initializes the JWT service with a secret key.
        /// </summary>
        /// <param name="key">Secret key for JWT encryption</param>
        public JwtService(string key)
        {
            _key = key;
        }

        /// <summary>
        /// Generates a JWT token for the given user ID.
        /// </summary>
        /// <param name="userId">User ID to include in the token claims</param>
        /// <returns>Generated JWT token as a string</returns>
        public string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
