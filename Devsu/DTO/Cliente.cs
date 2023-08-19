using System.ComponentModel.DataAnnotations;

namespace Devsu.DTO
{
    public class Cliente
    {
        [Required]
        public string Nombre { get; set; } = null!;
        
        [Required]
        public string Genero { get; set; } = null!;
        
        [Required]
        public int Edad { get; set; }
        
        [Required]
        public string Identificacion { get; set; } = null!;
        
        [Required]
        public string Direccion { get; set; } = null!;
        
        public string? Telefono { get; set; }
        
        [Required]
        public string Contraseña { get; set; } = null!;
        
        public bool? Estado { get; set; }
    }
}
