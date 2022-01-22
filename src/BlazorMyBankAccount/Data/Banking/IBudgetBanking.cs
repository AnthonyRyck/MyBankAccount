namespace BlazorMyBankAccount.Data.Banking
{
    public interface IBudgetBanking
    {
        /// <summary>
        /// Retourne le budget demandé.
        /// </summary>
        /// <returns></returns>
        Task<Budget> GetBudget(int idBudget);

        /// <summary>
        /// Retourne tous les budgets
        /// </summary>
        /// <returns></returns>
        Task<List<Budget>> GetBudgets();

        /// <summary>
        /// Retourne tous les comptes
        /// </summary>
        /// <returns></returns>
        Task<List<Compte>> GetAccounts();

        /// <summary>
        /// Retourne tous les types de budgets
        /// </summary>
        /// <returns></returns>
        Task<List<Typebudget>> GetTypesBudget();

        /// <summary>
        /// Ajoute un budget.
        /// </summary>
        /// <param name="budget">Un nouveau budget</param>
        /// <returns></returns>
        Task AddBudget(Budget budget);

        /// <summary>
        /// Ajout un nouveau budget
        /// </summary>
        /// <param name="budgetName">Nom du budget</param>
        /// <param name="description">Une description</param>
        /// <param name="compteId">Compte est stocke le budget</param>
        /// <param name="typebudget">Type de budget</param>
        /// <param name="montant">Montant pour dépense</param>
        /// <returns></returns>
        Task<Budget> AddBudget(string budgetName, string description, Compte compteId, Typebudget typebudget, decimal? montant);
    }
}
