using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Inventario
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        public TipoCategoria Tipo { get; set; } = TipoCategoria.DESCONOCIDO;

        [Required]
        public string Nombre { get; set; } = null!;
    }
}
