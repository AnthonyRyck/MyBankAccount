namespace BlazorMyBankAccount.Data.Banking
{
    public interface ICompteBanking
    {
        /// <summary>
        /// Retourne le compte demandé.
        /// </summary>
        /// <returns></returns>
        Task<Compte> GetAccount();
        
        /// <summary>
        /// Retourne tous les comptes
        /// </summary>
        /// <returns></returns>
        Task<List<Compte>> GetAccounts();

        /// <summary>
        /// Ajoute un compte.
        /// </summary>
        /// <param name="account">Un nouveau compte</param>
        /// <returns></returns>
        Task AddAccount(Compte account);

        /// <summary>
        /// Ajout un nouveau compte
        /// </summary>
        /// <param name="accountName">Nom du compte</param>
        /// <param name="description">Une description</param>
        /// <returns></returns>
        Task<Compte> AddAccount(string accountName, string description);
    }
}
