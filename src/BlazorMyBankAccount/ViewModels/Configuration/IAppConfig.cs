namespace BlazorMyBankAccount.ViewModels.Configuration
{
    public interface IAppConfig
    {
        /// <summary>
        /// Indicateur si les datas sont chargées
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// C'est la config en cours.
        /// </summary>
        Configbank LaConfiguration { get; }

        /// <summary>
        /// Permet au ViewModel d'indiquer un changement d'état.
        /// </summary>
        /// <param name="state"></param>
        void SetStateHasChanged(Action state);

        /// <summary>
        /// Récupère la configuration
        /// </summary>
        /// <returns></returns>
        Task InitData();

        /// <summary>
        /// Liste des comptes
        /// </summary>
        IEnumerable<Compte> Comptes { get; }

        /// <summary>
        /// Modele de validation
        /// </summary>
        ConfigValidation ValidationModel { get; }

        /// <summary>
        /// Indique si ouvrir le dialogue pour modification
        /// </summary>
        bool DialogIsOpen { get; }

        /// <summary>
        /// Pour ouvrir la boite de dialogue
        /// </summary>
        void OpenConfig();

        /// <summary>
        /// Valide la saisie
        /// </summary>
        void OnValidSubmit();

        /// <summary>
        /// Pour annuler la saisie
        /// </summary>
        void OnCancel();

        /// <summary>
        /// Retourne en toute lettre le mois de traitement.
        /// </summary>
        /// <returns></returns>
        string GetMois();

        /// <summary>
        /// Permet de changer du mois, passe au prochain.
        /// </summary>
        /// <returns></returns>
        Task OnClickChangeMois();
    }
}
