using System;
using System.Collections.Generic;

namespace Devsu.Entities
{
    public partial class Movimiento
    {
        public int Id { get; set; }
        public int Cuenta { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; } = null!;
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }

        public virtual Cuenta CuentaNavigation { get; set; } = null!;
    }
}
