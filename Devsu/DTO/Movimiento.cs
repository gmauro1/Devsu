using System.ComponentModel.DataAnnotations;

namespace Devsu.DTO
{
    public class Movimiento
    {
        [Required]
        public int Cuenta { get; set; }

        [Required]
        public decimal Valor { get; set; }
    }
}
