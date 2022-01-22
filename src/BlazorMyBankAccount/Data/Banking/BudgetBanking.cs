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

        /// <inheritdoc cref="IBudgetBanking.AddBudget(string, string, Compte)"/>
        public async Task<Budget> AddBudget(string budgetName, string description, Compte compteId)
        {
            Budget budget = new Budget()
            {
                Nombudget = budgetName,
                Description = description,
            };
            budget.Idcomptes.Add(compteId);

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
            //var test = context.Budgets.Join(context.Comptes,
            //                    bud => bud.Idcomptes,
            //                    cpt => cpt.Idcompte,
            //                    (bud, cpt) => cpt)
            //                .ToList();

            //var testJoin = (from bud in context.Budgets
            //                join cpt in context.Comptes
            //                on bud.Idbudget equals cpt.Idbudgets.Contains())


            return await context.Budgets.Include(cpt => cpt.Idcomptes).ToListAsync();
        }

        /// <inheritdoc cref="IBudgetBanking.GetAccounts"/>
        public async Task<List<Compte>> GetAccounts()
        {
            return await context.Comptes.ToListAsync();
        }

        #endregion

    }
}
