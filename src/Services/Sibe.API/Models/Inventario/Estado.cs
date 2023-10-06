using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sibe.API.Models.Inventario
{
    public class Estado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get { return _nombre; } set { _nombre = value.ToUpper(); } }

        [Required]
        public string Descripcion { get; set; } = string.Empty!;


        /* VARIABLES AUXILIARES */

        [NotMapped]
        private string _nombre = string.Empty;

    }
}