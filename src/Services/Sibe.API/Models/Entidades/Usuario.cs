using Sibe.API.Models.Enums;
using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Entidades
{
    // Clase para almacenar contraseñas anteriores con fecha de cambio
    public class HistoricoClave
    {
        [Key]
        public int Id { get; set; }

        public DateTime FechaRegistro { get; set; } = TimeZoneHelper.Now();

        public byte[] ClaveHash { get; set; } = Array.Empty<byte>();

        public byte[] ClaveSalt { get; set; } = Array.Empty<byte>();
    }

    public class Usuario
    {

        [Key]
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Nombre { get { return _nombre; } set { _nombre = value.ToUpper(); } }

        public Rol Rol { get; set; } = Rol.ASISTENTE;

        public string Correo { get; set; } = string.Empty;

        public string? ClaveTemporal { get; set; }

        public DateTime FechaRegistro { get; set; } = TimeZoneHelper.Now();

        public List<HistoricoClave> HistoricoClaves { get; set; } = new List<HistoricoClave>(); // Tabla para almacenar contraseñas anteriores


        [NotMapped]
        private string _nombre = string.Empty;

    }
}
