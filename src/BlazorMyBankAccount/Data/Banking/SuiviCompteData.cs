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

        /// <inheritdoc cref="ISuiviCompteData.((int)"/>
        public async Task<IEnumerable<Budget>> GetBudgets(int idCompte)
        {
            return await context.Budgets.Where(x => x.Idcompte == idCompte)
                                    .Include(x => x.Typebudget)
                                    .ToListAsync();
        }


        /// <inheritdoc cref="ISuiviCompteData.GetSuivicomptes(int, int, int, int?)"/>
        public async Task<List<Suivicompte>> GetSuivicomptes(int idCompte, int annnee, int mois, int? idBudget = null)
        {
            return await context.Suivicomptes.Where(cpt => cpt.Idcompte == idCompte
                                                            && cpt.Idannee == annnee
                                                            && cpt.Idmois == mois
                                                            && cpt.Idbudget == idBudget).ToListAsync();
        }

        /// <inheritdoc cref="ISuiviCompteData.GetMontantBudgetValide(int, int, int)"/>
        public async Task<decimal> GetMontantBudgetValide(int idcompte, int annee, int mois)
        {
            return await context.Suivicomptes.Where(cpt => cpt.Idcompte == idcompte
                                                            && cpt.Idannee == annee
                                                            && cpt.Idmois == mois
                                                            && cpt.Idbudget != null
                                                            && cpt.Isvalidate)
                                            .SumAsync(x => x.Montant);
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

        /// <inheritdoc cref="ISuiviCompteData.GetConfig"/>
        public async Task<Configbank> GetConfig()
        {
            return await context.Configbanks.Include(cpt => cpt.IdcomptedefaultNavigation)
                            .FirstOrDefaultAsync();
        }


        public async Task AddVirement(Suivicompte comptePerdant, Suivicompte compteRecevant)
		{
            await context.AddAsync(comptePerdant);
            await context.AddAsync(compteRecevant);
            await context.SaveChangesAsync();
        }

        public async Task AddTransactionObligatoire(Transactionobligatoire newTransac)
        {
            await context.AddAsync(newTransac);
            await context.SaveChangesAsync();
        }

        public async Task Validate(Suivicompte ligneSuivi, bool isValidate)
        {
            ligneSuivi.Isvalidate = isValidate;
            context.Update(ligneSuivi);
            await context.SaveChangesAsync();
        }

        public async Task UpdateLigne(Suivicompte suivicompte)
        {
            context.Update(suivicompte);
            await context.SaveChangesAsync();
        }
    }
}
