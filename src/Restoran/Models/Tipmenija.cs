using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public partial class Tipmenija
    {
        public Tipmenija()
        {
            Meni = new HashSet<Meni>();
        }

        public int Id { get; set; }
        public string Naziv { get; set; }

        public virtual ICollection<Meni> Meni { get; set; }
    }
}
