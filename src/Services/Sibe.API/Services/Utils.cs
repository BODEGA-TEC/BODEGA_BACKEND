using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sibe.API.Utils
{
    public static class TimeZoneHelper
    {
        public static DateTime Now()
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");
            DateTime currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            return currentTime;
        }
    }

    public static class RegexValidator
    {
        // Función para verificar que una cadena cumple con un patrón regex dado.
        public static void ValidateWithRegex(string value, string regexPattern, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException(errorMessage);
            }

            if (!Regex.IsMatch(value, regexPattern))
            {
                throw new ValidationException(errorMessage);
            }
        }
    }

    //[System.Runtime.Versioning.SupportedOSPlatform("linux")]
    public static class UniqueIdentifierHelper
    {
        public static string GenerateIdentifier(string inputString, int digit, int paddingSize)
        {
            // Formatear el dígito como una cadena con la cantidad de dígitos especificada, rellenando con ceros a la izquierda
            string formattedDigit = digit.ToString("D" + paddingSize);

            // Concatenar la cadena y el dígito formateado
            string code = inputString.ToUpper() + formattedDigit;

            return code;
        }

        public static string GenerateRandomString(int length)
        {
            //const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string allowedChars = "0123456789";

            // Obtiene el timestamp actual en milisegundos
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            // Inicializa el generador de números aleatorios
            Random random = new();

            // Crea un string aleatorio combinando el timestamp y caracteres permitidos
            string randomString = timestamp.ToString() +
                new string(Enumerable.Repeat(allowedChars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }
    }
}