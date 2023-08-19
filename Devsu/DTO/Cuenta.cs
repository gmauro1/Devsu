using System.ComponentModel.DataAnnotations;

namespace Devsu.DTO
{
    public class Cuenta
    {
        [Required]
        public int Cliente { get; set; }

        [Required]
        public string NumeroCuenta { get; set; } = null!;
        
        [Required]
        public string TipoCuenta { get; set; } = null!;
        
        [Required]
        public decimal Saldo { get; set; }
        
        [Required]
        public bool? Estado { get; set; }
    }
}
