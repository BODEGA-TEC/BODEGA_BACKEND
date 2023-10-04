using Sibe.API.Utils;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Entidades
{
    public class Usuario
    {
        /* CONSTANTES */

        private static readonly string emailRegexPattern = @"^[a-zA-Z0-9._%+-]+@estudiantec\.cr$";
        private static readonly string passwordRegex = "^(?=.*[a-zA-Z])(?=.*\\d)[a-zA-Z\\d\\/%()_\\-*&@]{4,16}$";


        /* ATRIBUTOS */

        [Key]
        [MaxLength(10)] // El carné tiene una longitud máxima de 10 caracteres.
        public string Carne { get; set; } = null!;

        // Navegación a la entidad Rol que representa el rol del usuario.
        [Required]
        public Rol Rol { get; set; } = null!;

        [Required]
        public string Nombre { get { return _nombre; } set { _nombre = value.ToUpper(); } }

        public byte[] ClaveHash { get; set; } = Array.Empty<byte>();

        public byte[] ClaveSalt { get; set; } = Array.Empty<byte>();

        [Required]
        [EmailAddress]
        public string Correo { get { return _correo; } set { SetValidCorreo(value); } }



        /* VARIABLES AUXILIARES */

        [NotMapped]
        public string Clave { get { return _clave; } set { SetValidClave(value); } }

        [NotMapped]
        private string _nombre = string.Empty;

        [NotMapped]
        private string _correo = string.Empty;

        [NotMapped]
        private string _clave = string.Empty;


        /* METODOS */

        // Funcion para verificar que la contraseña es de un formato correcto.
        private void SetValidClave(string clave)
        {
            string errorMessage = "La contraseña debe tener entre 4 y 16 caracteres, contener al menos una letra y un número, y puede incluir los caracteres permitidos: /, %, (, ), _, -, *, &, @.";

            RegexValidator.ValidateWithRegex(clave, passwordRegex, errorMessage);
            _clave = clave;
        }

        // Función para verificar y asignar un correo electrónico con formato válido.
        private void SetValidCorreo(string correo)
        {
            RegexValidator.ValidateWithRegex(correo, emailRegexPattern, "El correo debe tener el dominio @estudiantec.cr");
            _correo = correo.ToLower(); // Convierte a minúsculas antes de asignar
        }
    }
}
