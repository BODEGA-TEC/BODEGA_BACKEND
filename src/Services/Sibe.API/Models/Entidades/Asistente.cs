using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Entidades
{
    public class Asistente
    {
        private static readonly string emailRegexPattern = @"^[a-zA-Z0-9._%+-]+@estudiantec\.cr$";

        [Key]
        [MaxLength(10)] // El carné tiene una longitud máxima de 10 caracteres.
        public string Carne { get; set; } = string.Empty;

        public string Nombre { get { return _nombre; } set { _nombre = FormatNombre(value); } }


        [EmailAddress]
        public required string Correo { get { return _correo; } set { SetValidCorreo(value); } }

        public DateTime FechaRegistro { get; set; } = TimeZoneHelper.Now();

        public string HuellaDigital { get; set; } = string.Empty;

        // Propiedad calculada para determinar si el asistente está activo
        public bool Activo => (TimeZoneHelper.Now() - FechaRegistro).TotalDays <= 6 * 30;


        [NotMapped]
        private string _nombre = string.Empty;

        [NotMapped]
        private string _correo = string.Empty;


        private static string FormatNombre(string nombre)
        {
            if (!string.IsNullOrEmpty(nombre))
            {
                nombre = nombre.Trim(); // Elimina espacios en blanco del principio y final
                nombre = System.Text.RegularExpressions.Regex.Replace(nombre, @"\s+", " "); // Reduce espacios internos a uno
                nombre = nombre.ToUpper(); // Convierte a mayúsculas
            }

            return nombre;
        }

        // Función para verificar y asignar un correo electrónico con formato válido.
        private void SetValidCorreo(string correo)
        {
            RegexValidator.ValidateWithRegex(correo, emailRegexPattern, "El dominio debe ser @estudiantec.cr.");
            _correo = correo.ToLower(); // Convierte a minúsculas antes de asignar
        }
    }
}
