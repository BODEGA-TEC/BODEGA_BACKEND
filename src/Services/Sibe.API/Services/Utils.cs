using IronBarCode;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Processing;
//using SixLabors.ImageSharp.Formats.Jpeg;

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
        public static string GenerateIdentifier(int id, DateTime timestamp)
        {
            // Concatena el id y el timestamp como una cadena
            string input = id.ToString() + timestamp.ToString("yyyyMMddHHmmss");

            // Toma los primeros 12 caracteres de la cadena resultante
            string uniqueCode = input[..12];

            return uniqueCode;
        }

        public static string GenerateBarcode(string data)
        {
            // Crear objeto BarcodeGenerator
            var barcode = IronBarCode.BarcodeWriter.CreateBarcode(data, IronBarCode.BarcodeEncoding.Code128);
            //barcode.AddAnnotationTextAboveBarcode("activo bodega");
            barcode.AddBarcodeValueTextBelowBarcode();
            barcode.SetMargins(5, 5, 5, 5);
            barcode.ResizeTo(500, 200);

            // Guardar la imagen del código de barras en un archivo temporal
            string tempFile = Path.GetTempFileName();
            barcode.SaveAsPng(tempFile);

            // Leer el archivo temporal como una cadena Base64
            byte[] imageBytes = File.ReadAllBytes(tempFile);
            string base64String = Convert.ToBase64String(imageBytes);

            // Eliminar el archivo temporal
            File.Delete(tempFile);

            return base64String;
        }
    }
}