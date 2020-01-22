using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public partial class Grad
    {
        public Grad()
        {
            Zaposleni = new HashSet<Zaposleni>();
        }

        public int Id { get; set; }
        public int DrzavaId { get; set; }
        public string Naziv { get; set; }
        public string PostanskiBroj { get; set; }

        public virtual Drzava Drzava { get; set; }
        public virtual ICollection<Zaposleni> Zaposleni { get; set; }
    }
}
