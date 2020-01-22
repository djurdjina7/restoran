using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public partial class Spisakzanarudzbu
    {
        public int MeniId { get; set; }
        public int NarudzbaId { get; set; }
        public int? Kolicina { get; set; }
        public decimal? Cijena { get; set; }

        public virtual Meni Meni { get; set; }
        public virtual Narudzba Narudzba { get; set; }
    }
}
