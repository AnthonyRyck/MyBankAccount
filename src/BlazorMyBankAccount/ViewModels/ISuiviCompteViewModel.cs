namespace BlazorMyBankAccount.ViewModels
{
    public interface ISuiviCompteViewModel
    {
        /// <summary>
        /// Liste de tous les comptes
        /// </summary>
        IEnumerable<Compte> Comptes { get; }

        /// <summary>
        /// Compte sélectionné.
        /// </summary>
        Compte CompteSelected { get; set; }

        /// <summary>
        /// Liste de tous les budgets sur ce compte.
        /// </summary>
        IEnumerable<Budget> Budgets { get; }

        /// <summary>
        /// Liste de toutes les transactions sur un compte
        /// </summary>
        List<Suivicompte> SuiviDuCompte{ get; }

        /// <summary>
        /// Liste de tous les types de transactions
        /// </summary>
        IEnumerable<Typestransaction> TypesTransaction { get; }

        /// <summary>
        /// Indique un chargement de donnée.
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Indique s'il y a une configuration
        /// </summary>
        bool HasConfig { get; }

        /// <summary>
        /// Modèle de validation
        /// </summary>
        TransactionValidation TransacValidation { get; }

        RadzenGrid<Suivicompte> SaisieGrid { get; set; }

        /// <summary>
        /// Initialise les donnees.
        /// </summary>
        Task InitData();

        /// <summary>
        /// Sélection du compte pour affichage.
        /// </summary>
        /// <param name="compte"></param>
        /// <returns></returns>
        Task OnSelectCompte(object compte);

        /// <summary>
        /// Valide la saisie
        /// </summary>
        void OnValidSubmit();

        /// <summary>
        /// Annule la saisie
        /// </summary>
        void AnnulerSaisie();

    }
}
