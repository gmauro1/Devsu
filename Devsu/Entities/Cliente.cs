using System;
using System.Collections.Generic;

namespace Devsu.Entities
{
    public partial class Cliente
    {
        public Cliente()
        {
            Cuenta = new HashSet<Cuenta>();
        }

        public int Id { get; set; }
        public int Persona { get; set; }
        public string Contraseña { get; set; } = null!;
        public bool? Estado { get; set; }

        public virtual Persona PersonaNavigation { get; set; } = null!;
        public virtual ICollection<Cuenta> Cuenta { get; set; }
    }
}
