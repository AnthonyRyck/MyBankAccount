namespace BlazorMyBankAccount.Data.Banking
{
    public interface ITransactionData
    {

        /// <summary>
        /// Récupère toutes les transactions
        /// </summary>
        /// <returns></returns>
        Task<List<Transactionobligatoire>> GetTransactions();

        /// <summary>
        /// Recupere tous les comtpes
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Compte>> GetAllComptes();
        
        /// <summary>
        /// Recupere tous les types de transactions
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Typestransaction>> GetAllTypes();


        Task<Transactionobligatoire> AddTransaction(string nomTransaction, decimal montant, int jour, int idCompte, Typestransaction typetransac);

    }
}
