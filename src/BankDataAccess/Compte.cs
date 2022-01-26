using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Compte
    {
        public Compte()
        {
            Budgets = new HashSet<Budget>();
            Configbanks = new HashSet<Configbank>();
            Suivicomptes = new HashSet<Suivicompte>();
            Transactionobligatoires = new HashSet<Transactionobligatoire>();
        }

        public int Idcompte { get; set; }
        public string Nomcompte { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Configbank> Configbanks { get; set; }
        public virtual ICollection<Suivicompte> Suivicomptes { get; set; }
        public virtual ICollection<Transactionobligatoire> Transactionobligatoires { get; set; }
    }
}
