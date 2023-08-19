using System.ComponentModel.DataAnnotations;

namespace Devsu.DTO
{
    public class MovimientoResponse
    {
        public int Cuenta { get; set; }

        public decimal Valor { get; set; }

        public decimal Saldo { get; set; }

        public DateTime Fecha { get; set; }

        public string TipoMovimiento { get; set; } = default!;
    }
}
