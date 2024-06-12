using iTextSharp.text;
using iTextSharp.text.pdf;
using Sibe.API.Models.Boletas;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
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

        public static string GetSemesterPeriod(DateTime date)
        {
            int year = date.Year;
            int semester = date.Month <= 6 ? 1 : 2;
            return $"{semester} {year}";
        }

    }

    public class PageEventHelper : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED), 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER,
                "" + writer.PageNumber,
                (document.Right + document.Left) / 2,
                document.Bottom - 50,
                0);
            cb.EndText();
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

            Random random = new();
            string randomString = new string(Enumerable.Repeat(allowedChars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

            return randomString;
        }
    }

    public static class EmailHelper
    {
        public static string ObfuscateEmail(string email)
        {
            var atIndex = email.IndexOf('@');
            if (atIndex == -1)
                return email;  // Retorna el correo como está si no tiene '@'

            var prefix = email.Substring(0, atIndex);
            var domain = email.Substring(atIndex);

            if (prefix.Length <= 4)
                return email; // Si el prefijo es muy corto, retorna el correo como está

            var visiblePrefix = prefix.Substring(0, 3);  // Muestra los primeros tres caracteres
            var lastChar = prefix[prefix.Length - 1];  // Muestra el último carácter antes del '@'

            // Usa dos asteriscos para reemplazar la parte intermedia
            var obfuscatedPrefix = visiblePrefix + "**" + lastChar;

            return obfuscatedPrefix + domain;
        }
    }
}