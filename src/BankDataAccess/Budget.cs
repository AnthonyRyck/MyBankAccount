using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Budget
    {
        public Budget()
        {
            Idcomptes = new HashSet<Compte>();
        }

        public int Idbudget { get; set; }
        public string Nombudget { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Typebudgetid { get; set; }
        public decimal? Montant { get; set; }

        public virtual Typebudget Typebudget { get; set; } = null!;

        public virtual ICollection<Compte> Idcomptes { get; set; }
    }
}
