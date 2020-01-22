using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public partial class Sto
    {
        public Sto()
        {
            Narudzba = new HashSet<Narudzba>();
            Rezervacija = new HashSet<Rezervacija>();
        }

        public int Id { get; set; }
        public int? BrojMjesta { get; set; }
        public int? BrojStola { get; set; }
        public short? Dostupan { get; set; }

        public virtual ICollection<Narudzba> Narudzba { get; set; }
        public virtual ICollection<Rezervacija> Rezervacija { get; set; }
    }
}
