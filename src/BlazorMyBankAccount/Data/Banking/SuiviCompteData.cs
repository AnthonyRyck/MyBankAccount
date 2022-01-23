namespace BlazorMyBankAccount.Data.Banking
{
    public class SuiviCompteData : BankingData, ISuiviCompteData
    {
        public SuiviCompteData(bankingContext context)
            : base(context)
        {
        }

        /// <inheritdoc cref="ISuiviCompteData.GetComptes"/>
        public async Task<IEnumerable<Compte>> GetComptes()
        {
            return await context.Comptes.ToListAsync();
        }

        /// <inheritdoc cref="ISuiviCompteData.GetCompte(int)"/>
        public async Task<Compte> GetCompte(int idCompte)
        {
            return await context.Comptes.FirstOrDefaultAsync(x => x.Idcompte == idCompte);
        }


        /// <inheritdoc cref="ISuiviCompteData.GetSuivicomptes(int, int, int)"/>
        public async Task<List<Suivicompte>> GetSuivicomptes(int idCompte, int annnee, int mois)
        {
            return await context.Suivicomptes.Where(cpt => cpt.Idcompte == idCompte
                                                            && cpt.Idannee == annnee
                                                            && cpt.Idmois == mois).ToListAsync();
        }

        /// <inheritdoc cref="ISuiviCompteData.GetTypesTransaction"/>
        public async Task<IEnumerable<Typestransaction>> GetTypesTransaction()
        {
            return await context.Typestransactions.ToListAsync();
        }

        /// <inheritdoc cref="ISuiviCompteData.AddNouvelleSaisie(Suivicompte)"/>
        public async Task AddNouvelleSaisie(Suivicompte nouvelleEntre)
        {
            await context.AddAsync(nouvelleEntre);
            await context.SaveChangesAsync();
        }

    }
}
