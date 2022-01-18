using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Moistraitement
    {
        public Moistraitement()
        {
            Suivicomptes = new HashSet<Suivicompte>();
        }

        public int Mois { get; set; }

        public virtual ICollection<Suivicompte> Suivicomptes { get; set; }
    }
}
