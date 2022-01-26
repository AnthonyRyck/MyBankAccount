using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Budget
    {
        public int Idbudget { get; set; }
        public string Nombudget { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Idcompte { get; set; }
        public int Typebudgetid { get; set; }
        public decimal? Montant { get; set; }

        public virtual Compte IdcompteNavigation { get; set; } = null!;
        public virtual Typebudget Typebudget { get; set; } = null!;
    }
}
