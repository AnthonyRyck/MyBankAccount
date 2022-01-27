using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Suivicompte
    {
        public int Idsuivi { get; set; }
        public int Idcompte { get; set; }
        public int Idannee { get; set; }
        public int Idmois { get; set; }
        public DateTime? Datetransaction { get; set; }
        public int Typeid { get; set; }
        public string? Nomtransaction { get; set; }
        public bool Isvalidate { get; set; }
        public decimal Montant { get; set; }
        public int? Idbudget { get; set; }

        public virtual Anneetraitement IdanneeNavigation { get; set; } = null!;
        public virtual Compte IdcompteNavigation { get; set; } = null!;
        public virtual Moistraitement IdmoisNavigation { get; set; } = null!;
        public virtual Typestransaction Type { get; set; } = null!;
    }
}
