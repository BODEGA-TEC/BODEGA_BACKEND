
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
}