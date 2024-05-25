using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsercizioNumeriPrimi.Models
{
    public class PrimeNumber
    {
        [Key]
        public int id { get; set; }
        public int Number { get; set; }
        public DateTime VerificationTime { get; set; }

        public override string ToString()
        {
            return $"{this.Number} è un numero primo ed è stato verificato il: {this.VerificationTime.ToString()}";
        }
    }
}
