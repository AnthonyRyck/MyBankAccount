namespace BlazorMyBankAccount.ViewModels
{
    public interface IHomePage
    {
        /// <summary>
        /// Indicateur si les donnees sont chargees.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Indique que la configuration est OK.
        /// </summary>
        bool HasConfig { get; }

        /// <summary>
        /// Charge toutes les données.
        /// </summary>
        /// <returns></returns>
        Task InitData();
    }
}
