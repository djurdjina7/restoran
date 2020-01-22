using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public partial class Zaposleni
    {
        public Zaposleni()
        {
            Narudzba = new HashSet<Narudzba>();
        }

        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public string MaticniBroj { get; set; }
        public int GradId { get; set; }
        public string Adresa { get; set; }

        public virtual Grad Grad { get; set; }
        public virtual ICollection<Narudzba> Narudzba { get; set; }
    }
}
