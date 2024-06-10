using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Entidades
{

    public class Solicitante
    {

        [Key]
        public required string Carne { get; set; }

        private string _nombre = string.Empty;

        public string Nombre
        {
            get => _nombre;
            set => _nombre = value?.ToUpperInvariant() ?? string.Empty;
        }

        public required string Correo { get; set; }
        public required string Carrera { get; set; }
        public TipoSolicitante Tipo { get; set; }


    }
}