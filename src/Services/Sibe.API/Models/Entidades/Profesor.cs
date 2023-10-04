using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Sibe.API.Models.Entidades
{
    public class Profesor
    {
        /* CONSTANTES */

        private static readonly string emailRegexPattern = @"^[a-zA-Z0-9._%+-]+@itcr\.ac\.cr$";


        /* ATRIBUTOS */

        [Key]
        public int Id { get; set; }

        // Navegación a la entidad Departamento que representa el area academica a la que pertenece el profesor
        [Required]
        public Departamento Departamento { get; set; } = null!;

        [Required]
        public string Nombre { get { return _nombre; } set { _nombre = value.ToUpper(); } }

        [Required]
        public string PrimerApellido { get; set; } = null!;

        [Required]
        public string SegundoApellido { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Correo { get { return _correo; } set { SetValidCorreo(value); } }


        /* VARIABLES AUXILIARES */

        [NotMapped]
        private string _nombre = string.Empty;

        [NotMapped]
        private string _correo = string.Empty;


        /* METODOS */

        // Función para verificar y asignar un correo electrónico con formato válido.
        private void SetValidCorreo(string correo)
        {
            RegexValidator.ValidateWithRegex(correo, emailRegexPattern, "El correo debe tener el dominio @itcr.ac.cr");
            _correo = correo.ToLower(); // Convierte a minúsculas antes de asignar
        }

    }
}