using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Entidades
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        [Required]
        public string Descripcion { get; set; } = null!;
    }
}
