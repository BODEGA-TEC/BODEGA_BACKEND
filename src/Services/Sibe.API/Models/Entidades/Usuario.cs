using Sibe.API.Models.Enums;
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

        [Key]
        public string Username { get; set; } = string.Empty;

        public Rol Rol { get; set; } = Rol.ASISTENTE;

        public string Correo { get; set; } = string.Empty;

        public string? ClaveTemporal { get; set; }

        public DateTime FechaRegistro { get; set; } = TimeZoneHelper.Now();

        public List<HistoricoClave> HistoricoClaves { get; set; } = new List<HistoricoClave>(); // Tabla para almacenar contraseñas anteriores


        [NotMapped]
        private string _nombre = string.Empty;

        [NotMapped]
        private string _correo = string.Empty;

    }
}
