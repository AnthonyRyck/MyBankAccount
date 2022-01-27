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
        /// <returns></returns>
        Task<Configbank> GetConfig();

        /// <summary>
        /// Ajoute une nouvelle entré dans le suivi.
        /// </summary>
        /// <param name="nouvelleEntre"></param>
        /// <returns></returns>
        Task AddNouvelleSaisie(Suivicompte nouvelleEntre);
    }
}
