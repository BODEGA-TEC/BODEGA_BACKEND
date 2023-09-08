using Sibe.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public TipoCategoria Tipo { get; set; } = TipoCategoria.DESCONOCIDO;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Descripcion { get; set; } = null!;
    }
}
