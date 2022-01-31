using BankDataAccess;

namespace BlazorMyBankAccount.Data.Banking
{
    public interface IConfigBanking
    {
        /// <summary>
        /// Récupère la configuration
        /// </summary>
        /// <returns></returns>
        Task <Configbank> GetConfiguration();

        /// <summary>
        /// Charge tous les comptes.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Compte>> GetComptes();

        /// <summary>
        /// Ajoute ou met à jour la ligne de configuration
        /// </summary>
        /// <param name="compteParDefault"></param>
        /// <param name="annee"></param>
        /// <param name="mois"></param>
        /// <returns></returns>
        Task<Configbank> AddOrUpdate(Compte compteParDefault, int annee, int mois);
        
        /// <summary>
        /// Indique la création d'un nouveau mois.
        /// </summary>
        /// <param name="idCompteDefaut"></param>
        /// <param name="annee"></param>
        /// <param name="mois"></param>
        /// <returns></returns>
        Task CreateNewMonth(int idCompteDefaut, int annee, int mois);
    }
}
