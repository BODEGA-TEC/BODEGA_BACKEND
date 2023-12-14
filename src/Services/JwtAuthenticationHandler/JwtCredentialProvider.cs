using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthenticationHandler
{
    public class JwtCredentialProvider
    {
        internal readonly string SECURITY_KEY;
        private readonly int ACCESS_TOKEN_EXPIRATION_MINUTES;
        private readonly int REFRESH_TOKEN_EXPIRATION_DAYS;

        public JwtCredentialProvider(IConfiguration configuration)
        {
            SECURITY_KEY = configuration["JwtSettings:SecurityKey"];

            if (string.IsNullOrEmpty(SECURITY_KEY))
            {
                throw new ArgumentException("La clave de seguridad JWT no está configurada en la aplicación.");
            }

            if (!int.TryParse(configuration["JwtSettings:AccessTokenExpirationMinutes"], out ACCESS_TOKEN_EXPIRATION_MINUTES))
            {
                throw new ArgumentException("La configuración del tiempo de expiración del access token JWT no es válida.");
            }

            if (!int.TryParse(configuration["JwtSettings:RefreshTokenExpirationDays"], out REFRESH_TOKEN_EXPIRATION_DAYS))
            {
                throw new ArgumentException("La configuración del tiempo de expiración del refresh token JWT no es válida.");
            }
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static bool AuthPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        public string CreateToken(string email, string nombre, List<string> roles)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nombre) || roles == null || !roles.Any())
            {
                throw new ArgumentException("email, name, and roles cannot be null or empty.");
            }

            // Add claims
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, email),
                    new Claim(ClaimTypes.Name, nombre)
                };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
            var signingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            var expirationTimestamp = DateTime.Now.AddMinutes(ACCESS_TOKEN_EXPIRATION_MINUTES);

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

        public (string Token, DateTime CreatedTime, DateTime ExpiredTime) GenerateRefreshToken()
        {
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            DateTime createdDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));
            DateTime expiredDate = createdDate.AddMinutes(REFRESH_TOKEN_EXPIRATION_DAYS);

            return (refreshToken, createdDate, expiredDate);
        }

    }
}
