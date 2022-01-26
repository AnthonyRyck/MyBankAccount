using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Configbank
    {
        public int Idcomptedefault { get; set; }
        public int Annee { get; set; }
        public int Mois { get; set; }

        public virtual Anneetraitement AnneeNavigation { get; set; } = null!;
        public virtual Compte IdcomptedefaultNavigation { get; set; } = null!;
        public virtual Moistraitement MoisNavigation { get; set; } = null!;
    }
}
