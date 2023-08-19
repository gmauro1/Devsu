using System.ComponentModel.DataAnnotations;

namespace Devsu.DTO
{
    public class Reporte
    {
        public string Fecha { get; set; } = default!;

        public string Cliente { get; set; } = default!;

        public string NumeroCuenta { get; set; } = default!;

        public string Tipo { get; set; } = default!;

        public decimal SaldoInicial { get; set; }

        public bool Estado { get; set; }
        
        public decimal Movimiento { get; set; }

        public decimal SaldoDisponible { get; set; }
    }
}
