using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restoran.Models
{
    public partial class Rezervacija
    {
        public int Id { get; set; }
        public int StoId { get; set; }
        public string PodaciGosta { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Datum { get; set; }
        [DataType(DataType.Time)]
        public DateTime? VrijemeOd { get; set; }
        [DataType(DataType.Time)]
        public DateTime? VrijemeDo { get; set; }
        public int? BrojOsoba { get; set; }

        public virtual Sto Sto { get; set; }
    }
}
