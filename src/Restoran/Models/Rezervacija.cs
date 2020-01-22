using System;
using System.Collections.Generic;

namespace Restoran.Models
{
    public partial class Rezervacija
    {
        public int Id { get; set; }
        public int StoId { get; set; }
        public string PodaciGosta { get; set; }
        public DateTime? Datum { get; set; }
        public DateTimeOffset? VrijemeOd { get; set; }
        public DateTimeOffset? VrijemeDo { get; set; }
        public int? BrojOsoba { get; set; }

        public virtual Sto Sto { get; set; }
    }
}
