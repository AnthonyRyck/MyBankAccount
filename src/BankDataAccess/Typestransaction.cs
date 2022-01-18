using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Typestransaction
    {
        public Typestransaction()
        {
            Suivicomptes = new HashSet<Suivicompte>();
            Transactionobligatoires = new HashSet<Transactionobligatoire>();
        }

        public int Idtype { get; set; }
        public string Nom { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Suivicompte> Suivicomptes { get; set; }
        public virtual ICollection<Transactionobligatoire> Transactionobligatoires { get; set; }
    }
}
