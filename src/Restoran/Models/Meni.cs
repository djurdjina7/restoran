using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public partial class Meni
    {
        public Meni()
        {
            Spisakzanarudzbu = new HashSet<Spisakzanarudzbu>();
        }

        public int Id { get; set; }
        public int TipMenijaId { get; set; }
        public string Naziv { get; set; }
        public decimal? Cijena { get; set; }
        public int? Kolicina { get; set; }

        public virtual Tipmenija TipMenija { get; set; }
        public virtual ICollection<Spisakzanarudzbu> Spisakzanarudzbu { get; set; }
    }
}
