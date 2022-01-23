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
        /// Retourne le compte demandé.
        /// </summary>
        /// <returns></returns>
        Task<Compte> GetCompte(int idCompte);

        /// <summary>
        /// Retourne le suivi du compte donné.
        /// </summary>
        /// <param name="idCompte">ID du compte</param>
        /// <param name="annnee">Année du suivi</param>
        /// <param name="mois">Mois du suivi</param>
        /// <returns></returns>
        Task<List<Suivicompte>> GetSuivicomptes(int idCompte, int annnee, int mois);
        
        /// <summary>
        /// Retourne tous les types de transaction.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Typestransaction>> GetTypesTransaction();
        
        /// <summary>
        /// Ajoute une nouvelle entré dans le suivi.
        /// </summary>
        /// <param name="nouvelleEntre"></param>
        /// <returns></returns>
        Task AddNouvelleSaisie(Suivicompte nouvelleEntre);
    }
}
