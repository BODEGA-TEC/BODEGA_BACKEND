using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Inventario
{
    public class Estado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty!;
    }
}