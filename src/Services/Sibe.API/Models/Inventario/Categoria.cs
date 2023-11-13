using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Inventario
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        public TipoActivo Tipo { get; set; } = TipoActivo.DESCONOCIDO;

        [Required]
        public string Nombre { get; set; } = null!;
    }
}
