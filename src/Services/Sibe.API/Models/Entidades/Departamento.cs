using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models.Entidades
{
    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;
    }
}
