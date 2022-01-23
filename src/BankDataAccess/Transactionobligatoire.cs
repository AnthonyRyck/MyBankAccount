using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Transactionobligatoire
    {
        public int Idtransac { get; set; }
        public int Idcompte { get; set; }
        public string Nomtransaction { get; set; } = null!;
        public decimal Montant { get; set; }
        public int Typeid { get; set; }
        public int Jour { get; set; }

        public virtual Compte IdcompteNavigation { get; set; } = null!;
        public virtual Typestransaction Type { get; set; } = null!;
    }
}
