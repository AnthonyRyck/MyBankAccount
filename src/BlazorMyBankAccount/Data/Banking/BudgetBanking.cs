namespace BlazorMyBankAccount.Data.Banking
{
    public class BudgetBanking : BankingData, IBudgetBanking
    {
        public BudgetBanking(bankingContext dbContext)
        : base(dbContext)
        {
        }

        #region Implement IBudgetBanking

        /// <inheritdoc cref="IBudgetBanking.AddBudget(Budget)"/>
        public async Task AddBudget(Budget budget)
        {
            await context.Budgets.AddAsync(budget);
            await context.SaveChangesAsync();
        }

        /// <inheritdoc cref="IBudgetBanking.AddBudget(string, string, Compte, Typebudget)"/>
        public async Task<Budget> AddBudget(string budgetName, string description, Compte compteId, Typebudget typebudget, decimal? montant)
        {
            Budget budget = new Budget()
            {
                Nombudget = budgetName,
                Description = description,
                Montant = montant
            };
            budget.Idcompte = compteId.Idcompte;
            budget.Typebudget = typebudget;

            await context.Budgets.AddAsync(budget);
            await context.SaveChangesAsync();

            return budget;
        }

        /// <inheritdoc cref="IBudgetBanking.GetBudget(int)"/>
        public async Task<Budget> GetBudget(int idBudget)
        {
            return await context.Budgets.FirstOrDefaultAsync(x => x.Idbudget == idBudget);
        }

        /// <inheritdoc cref="IBudgetBanking.GetBudgets"/>
        public async Task<List<Budget>> GetBudgets()
        {
            return await context.Budgets.Include(cpt => cpt.IdcompteNavigation)
                                        .Include(typ => typ.Typebudget)
                                        .ToListAsync();
        }

        /// <inheritdoc cref="IBudgetBanking.GetAccounts"/>
        public async Task<List<Compte>> GetAccounts()
        {
            return await context.Comptes.ToListAsync();
        }

        /// <inheritdoc cref="IBudgetBanking.GetTypesBudget"/>
        public async Task<List<Typebudget>> GetTypesBudget()
        {
            return await context.Typebudgets.ToListAsync();
        }

        #endregion

    }
}
