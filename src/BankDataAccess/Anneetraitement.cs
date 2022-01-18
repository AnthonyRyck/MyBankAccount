using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Anneetraitement
    {
        public Anneetraitement()
        {
            Suivicomptes = new HashSet<Suivicompte>();
        }

        public int Annee { get; set; }

        public virtual ICollection<Suivicompte> Suivicomptes { get; set; }
    }
}
