using System.ComponentModel.DataAnnotations;

namespace Sibe.API.Models
{
    public class Estado
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
    }
}