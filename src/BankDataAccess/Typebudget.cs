using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Typebudget
    {
        public Typebudget()
        {
            Budgets = new HashSet<Budget>();
        }

        public int Idtypebudget { get; set; }
        public string Nomtypebudget { get; set; } = null!;

        public virtual ICollection<Budget> Budgets { get; set; }
    }
}
