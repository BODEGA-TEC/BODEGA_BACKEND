using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthenticationHandler
{
    /// <summary>
    /// Proveedor de credenciales JWT que gestiona la configuración relacionada con la autenticación JWT.
    /// </summary>
    public class JwtCredentialProvider
    {
        internal readonly string SECURITY_KEY;
        private readonly int USER_ACCESS_TOKEN_EXPIRATION_MINUTES;
        private readonly int USER_REFRESH_TOKEN_EXPIRATION_HOURS;


        /// <summary>
        /// Inicializa una nueva instancia de la clase JwtCredentialProvider.
        /// </summary>
        /// <param name="configuration">La configuración de la aplicación.</param>
        /// <exception cref="ArgumentException">Se lanza si la configuración de la aplicación es incorrecta o incompleta.</exception>
        public JwtCredentialProvider(IConfiguration configuration)
        {
            SECURITY_KEY = configuration["JwtSettings:SecurityKey"];

            if (string.IsNullOrEmpty(SECURITY_KEY))
            {
                throw new ArgumentException("La clave de seguridad JWT no está configurada en la aplicación.");
            }

            if (!int.TryParse(configuration["JwtSettings:UserAccessTokenExpirationMinutes"], out USER_ACCESS_TOKEN_EXPIRATION_MINUTES))
            {
                throw new ArgumentException("La configuración del tiempo de expiración del access token JWT no es válida.");
            }

            if (!int.TryParse(configuration["JwtSettings:UserRefreshTokenExpirationHours"], out USER_REFRESH_TOKEN_EXPIRATION_HOURS))
            {
                throw new ArgumentException("La configuración del tiempo de expiración del refresh token JWT no es válida.");
            }
        }


        /** 
         * USUARIO AUTH 
         **/


        /// <summary>
        /// Crea un hash y una sal a partir de una contraseña utilizando el algoritmo HMACSHA512.
        /// </summary>
        /// <param name="password">La contraseña a partir de la cual se generará el hash.</param>
        /// <param name="passwordHash">El hash resultante de la contraseña.</param>
        /// <param name="passwordSalt">La sal utilizada en el proceso de hashing.</param>
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }


        /// <summary>
        /// Autentica una contraseña utilizando un hash y una sal previamente generados.
        /// </summary>
        /// <param name="password">La contraseña a autenticar.</param>
        /// <param name="passwordHash">El hash de la contraseña con el que se compara el hash calculado.</param>
        /// <param name="passwordSalt">La sal utilizada en el proceso de hashing.</param>
        /// <returns>Devuelve true si la contraseña es válida, de lo contrario, false.</returns>
        public static bool AuthPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }


        /// <summary>
        /// Crea un token JWT para un usuario con el nombre de usuario, el correo electrónico y el rol especificados.
        /// </summary>
        /// <param name="id">El id para el cual se crea el token.</param>
        /// <param name="username">El nombre de usuario para el cual se crea el token.</param>
        /// <param name="email">El correo electrónico asociado al usuario.</param>
        /// <param name="rol">El rol del usuario para el cual se crea el token.</param>
        /// <returns>El token JWT generado para el usuario.</returns>
        /// <exception cref="ArgumentException">Se lanza si el nombre de usuario, el correo electrónico o el rol son nulos o vacíos.</exception>
        public string CreateToken(int id, string username, string email, string rol)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(rol))
            {
                throw new ArgumentException("email, username, and rol cannot be null or empty.");
            }

            // Add claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, rol.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
            var signingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            var expirationTimestamp = DateTime.UtcNow.AddMinutes(USER_ACCESS_TOKEN_EXPIRATION_MINUTES);

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


        /// <summary>
        /// Verifica si un token JWT ha expirado.
        /// </summary>
        /// <param name="token">El token JWT a verificar.</param>
        /// <returns>True si el token ha expirado, de lo contrario, False.</returns>
        /// 
        public static bool IsTokenExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);
            return jwt.ValidTo < DateTime.UtcNow;
        }


        /// <summary>
        /// Genera un token de actualización único junto con la fecha y hora de creación y expiración.
        /// </summary>
        /// <returns>Una tupla que contiene el token de actualización, la fecha y hora de creación y la fecha y hora de expiración del token.</returns>
        public (string Token, DateTime CreatedTime, DateTime ExpiredTime) GenerateRefreshToken()
        {
            string refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            DateTime createdDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time"));
            DateTime expiredDate = createdDate.AddHours(USER_REFRESH_TOKEN_EXPIRATION_HOURS);

            return (refreshToken, createdDate, expiredDate);
        }


        /** 
         * ASISTENTE AUTH 
         **/


        /// <summary>
        /// Crea un hash y una salt a partir de un PIN utilizando el algoritmo RFC2898DeriveBytes con SHA512.
        /// </summary>
        /// <param name="pin">El PIN a partir del cual se generará el hash.</param>
        /// <param name="pinHash">El hash resultante del PIN.</param>
        /// <param name="pinSalt">El salt utilizado en el proceso de hashing.</param>
        public static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(pin, 16, 10000, HashAlgorithmName.SHA512))
            {
                pinSalt = deriveBytes.Salt;
                pinHash = deriveBytes.GetBytes(32); // 32 bytes for HMACSHA512
            }
        }


        /// <summary>
        /// Autentica un PIN utilizando un hash y una sal previamente generados.
        /// </summary>
        /// <param name="pin">El PIN a autenticar.</param>
        /// <param name="pinHash">El hash del PIN con el que se compara el hash calculado.</param>
        /// <param name="pinSalt">La sal utilizada en el proceso de hashing.</param>
        /// <returns>Devuelve true si el PIN es válido, de lo contrario, false.</returns>
        public static bool AuthPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(pin, pinSalt, 10000, HashAlgorithmName.SHA512))
            {
                var computedHash = deriveBytes.GetBytes(32); // 32 bytes for HMACSHA512
                return computedHash.SequenceEqual(pinHash);
            }
        }


        /// <summary>
        /// Crea un token JWT para un asistente utilizando un carné y un tiempo de expiración especificados.
        /// </summary>
        /// <param name="carne">El carné del asistente para el cual se crea el token.</param>
        /// <param name="expirationTime">El tiempo de expiración del token en minutos.</param>
        /// <returns>El token JWT generado para el asistente.</returns>
        /// <exception cref="ArgumentException">Se lanza si el carné es nulo o vacío.</exception>
        public string CreateAsistenteToken(string carne, int expirationTime)
        {
            if (string.IsNullOrEmpty(carne))
            {
                throw new ArgumentException("El carné no puede ser nulo o vacío.");
            }
            // Add claims
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, carne)
                };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY));
            var signingCreds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            var expirationTimestamp = DateTime.UtcNow.AddSeconds(expirationTime);

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


        /// <summary>
        /// Extrae un claim específico de un token JWT.
        /// </summary>
        /// <param name="token">El token JWT del cual extraer el claim.</param>
        /// <param name="claimType">El tipo de claim que se desea extraer.</param>
        /// <returns>El valor del claim especificado.</returns>
        /// <exception cref="ArgumentException">Se lanza cuando el claim especificado no se encuentra en el token o ha ocurrido un error relacionado con el token.</exception>
        /// <exception cref="Exception">Se lanza cuando se produce un error no especificado durante el proceso de autenticación.</exception>
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
