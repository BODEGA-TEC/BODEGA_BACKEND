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

            if (!int.TryParse(configuration["JwtSettings:RefreshTokenExpirationHours"], out REFRESH_TOKEN_EXPIRATION_DAYS))
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

        public string CreateToken(string username, string email, string rol)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(rol))
            {
                throw new ArgumentException("email, username, and rol cannot be null or empty.");
            }

            // Add claims
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, rol)
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
            var signingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            var expirationTimestamp = DateTime.UtcNow.AddMinutes(ACCESS_TOKEN_EXPIRATION_MINUTES);

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




        // Asistente AUTH
        public static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(pin, 16, 10000, HashAlgorithmName.SHA512))
            {
                pinSalt = deriveBytes.Salt;
                pinHash = deriveBytes.GetBytes(32); // 32 bytes for HMACSHA512
            }
        }

        public static bool AuthPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(pin, pinSalt, 10000, HashAlgorithmName.SHA512))
            {
                var computedHash = deriveBytes.GetBytes(32); // 32 bytes for HMACSHA512
                return computedHash.SequenceEqual(pinHash);
            }
        }

        public string CreateAsistenteToken(string carne, int expirationTime)
        {
            if (string.IsNullOrEmpty(carne))
            {
                throw new ArgumentException("carne cannot be null or empty.");
            }
            // Add claims
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, carne)
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
            var signingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            var expirationTimestamp = DateTime.UtcNow.AddMinutes(expirationTime);

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

        public string ExtractClaimsFromToken(string token, string claimType)
        {
            try 
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                // Acceder al claim
                var claim = claimsPrincipal.FindFirst(claimType) ?? throw new ArgumentException($"Ha ocurrido un error con el claim '{claimType}'.");

                // Retornar el valor del claim si existe
                return claim.Value;
            }

            catch (SecurityTokenExpiredException)
            {
                throw new ArgumentException(" El token ha expirado.");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                throw new ArgumentException("La firma del token no es válida.");
            }
            catch (Exception)
            {
                throw new Exception("Ha ocurrido un error con la autenticación.");
            }
        }


    }
}
