using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.DTO
{
    public class VillaDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Nombre { get; set; }

        public int Ocupantes { get; set; }
        public int MetrosCuadrados { get; set; }
    }
}
