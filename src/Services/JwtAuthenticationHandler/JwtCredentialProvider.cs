using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthenticationHandler
{
    public class JwtCredentialProvider
    {
        internal readonly string SECURITY_KEY;
        private readonly int TOKEN_EXPIRATION_MINUTES;

        public JwtCredentialProvider(IConfiguration configuration)
        {
            SECURITY_KEY = configuration["JwtSettings:SecurityKey"];

            if (string.IsNullOrEmpty(SECURITY_KEY))
            {
                throw new ArgumentException("La clave de seguridad JWT no está configurada en la aplicación.");
            }

            if (!int.TryParse(configuration["JwtSettings:TokenExpirationMinutes"], out TOKEN_EXPIRATION_MINUTES))
            {
                throw new ArgumentException("La configuración del tiempo de expiración del token JWT no es válida.");
            }
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static bool AuthPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        public string CreateToken(string carne, string nombre, string role)
        {

            if (string.IsNullOrEmpty(carne) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(role))
            {
                throw new ArgumentException("uid, username, and role cannot be null or empty.");
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, carne),
                    new Claim(ClaimTypes.Name, nombre),
                    new Claim(ClaimTypes.Role, role)
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
            var signingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            var expirationTimestamp = DateTime.Now.AddDays(TOKEN_EXPIRATION_MINUTES);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationTimestamp,
                SigningCredentials = signingCreds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
