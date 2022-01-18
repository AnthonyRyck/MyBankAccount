using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Compte
    {
        public Compte()
        {
            Suivicomptes = new HashSet<Suivicompte>();
            Transactionobligatoires = new HashSet<Transactionobligatoire>();
            Idbudgets = new HashSet<Budget>();
        }

        public int Idcompte { get; set; }
        public string Nomcompte { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Suivicompte> Suivicomptes { get; set; }
        public virtual ICollection<Transactionobligatoire> Transactionobligatoires { get; set; }

        public virtual ICollection<Budget> Idbudgets { get; set; }
    }
}
