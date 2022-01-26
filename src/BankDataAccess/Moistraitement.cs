using System;
using System.Collections.Generic;

namespace BankDataAccess
{
    public partial class Moistraitement
    {
        public Moistraitement()
        {
            Configbanks = new HashSet<Configbank>();
            Suivicomptes = new HashSet<Suivicompte>();
        }

        public int Mois { get; set; }

        public virtual ICollection<Configbank> Configbanks { get; set; }
        public virtual ICollection<Suivicompte> Suivicomptes { get; set; }
    }
}
