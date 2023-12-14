using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Entidades
{
    // Clase para almacenar contraseñas anteriores con fecha de cambio
    public class HistoricoClave
    {
        private static readonly string passwordRegex = "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d\\/%()_\\-*&@]{8,16}$";


        [Key]
        public int Id { get; set; }

        public DateTime FechaCambio { get; set; } = TimeZoneHelper.Now();

        public byte[] ClaveHash { get; set; } = Array.Empty<byte>();

        public byte[] ClaveSalt { get; set; } = Array.Empty<byte>();


        [NotMapped]
        public string Clave { get { return _clave; } set { SetValidClave(value); } }

        [NotMapped]
        private string _clave = string.Empty;


        // Funcion para verificar que la contraseña es de un formato correcto.
        private void SetValidClave(string clave)
        {
            string errorMessage = @"De 8 a 16 caracteres.
                                    Al menos una letra mayúscula.
                                    Al menos una letra minúscula.
                                    Al menos un número.
                                    Al menos un carácter especial: "" /, %, (, ), _, -, *, &, @.""";

            RegexValidator.ValidateWithRegex(clave, passwordRegex, errorMessage);
            _clave = clave;
        }
    }

    public class Usuario
    {
        private static readonly string emailRegexPattern = @"^[a-zA-Z0-9._%+-]+@(estudiantec\.cr|itcr\.ac\.cr)$";


        [Key]
        public int Id { get; set; }

        public Escuela? Escuela { get; set; }

        public List<Rol> Roles { get; set; } = new List<Rol>();

        public required string Nombre { get { return _nombre; } set { _nombre = value.ToUpper(); } }

        [MaxLength(10)] // El carné tiene una longitud máxima de 10 caracteres.
        public string? Carne { get; set; }

        [EmailAddress]
        public required string Correo { get { return _correo; } set { SetValidCorreo(value); } }

        public string? ClaveTemporal { get; set; }

        public List<HistoricoClave> HistoricoClaves { get; set; } = new List<HistoricoClave>(); // Tabla para almacenar contraseñas anteriores


        [NotMapped]
        private string _nombre = string.Empty;

        [NotMapped]
        private string _correo = string.Empty;


        // Función para verificar y asignar un correo electrónico con formato válido.
        private void SetValidCorreo(string correo)
        {
            RegexValidator.ValidateWithRegex(correo, emailRegexPattern, "El dominio debe ser @estudiantec.cr o @itcr.ac.cr.");
            _correo = correo.ToLower(); // Convierte a minúsculas antes de asignar
        }
    }
}
