namespace BlazorMyBankAccount.Data.Banking
{
    public interface ISuiviCompteData
    {
        /// <summary>
        /// Retourne tous les comptes.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Compte>> GetComptes();

        /// <summary>
        /// Retourne les budgets sur le compte demandé.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Budget>> GetBudgets(int idCompte);

        /// <summary>
        /// Retourne le suivi du compte donné.
        /// </summary>
        /// <param name="idCompte">ID du compte</param>
        /// <param name="annnee">Année du suivi</param>
        /// <param name="mois">Mois du suivi</param>
        /// <param name="idBudget">ID du budget</param>
        /// <returns></returns>
        Task<List<Suivicompte>> GetSuivicomptes(int idCompte, int annnee, int mois, int? idBudget = null);
        
        /// <summary>
        /// Retourne tous les types de transaction.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Typestransaction>> GetTypesTransaction();
        
        /// <summary>
        /// Recupere la configuration.
        /// </summary>
        /// <returns>Retourne la config</returns>
        Task<Configbank> GetConfig();

        /// <summary>
        /// Ajoute une nouvelle entré dans le suivi.
        /// </summary>
        /// <param name="nouvelleEntre"></param>
        Task AddNouvelleSaisie(Suivicompte nouvelleEntre);

        /// <summary>
        /// Virement entre 2 comptes.
        /// </summary>
        /// <param name="comptePerdant"></param>
        /// <param name="compteRecevant"></param>
		Task AddVirement(Suivicompte comptePerdant, Suivicompte compteRecevant);

        /// <summary>
        /// Permet d'ajouter une nouvelle ligne dans les transactions
        /// obligatoire.
        /// </summary>
        /// <param name="newTransac"></param>
        Task AddTransactionObligatoire(Transactionobligatoire newTransac);
        
        /// <summary>
        /// Valide la transaction
        /// </summary>
        /// <param name="ligneSuivi"></param>
        /// <param name="isValidate"></param>
        /// <returns></returns>
        Task Validate(Suivicompte ligneSuivi, bool isValidate);
    }
}
