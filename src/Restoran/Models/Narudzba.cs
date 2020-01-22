using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public partial class Narudzba
    {
        public Narudzba()
        {
            Spisakzanarudzbu = new HashSet<Spisakzanarudzbu>();
        }

        public int Id { get; set; }
        public int ZaposleniId { get; set; }
        public int StoId { get; set; }
        public DateTime? VrijemeKreiranja { get; set; }
        public DateTime? VrijemeZavrsetka { get; set; }
        public decimal? Cijena { get; set; }

        public virtual Sto Sto { get; set; }
        public virtual Zaposleni Zaposleni { get; set; }
        public virtual ICollection<Spisakzanarudzbu> Spisakzanarudzbu { get; set; }
    }
}
