namespace BlazorMyBankAccount.ViewModels.Configuration
{
    public interface ICompteConfig
    {
		/// <summary>
		/// Tous les comptes
		/// </summary>
		List<Compte> AllAccounts { get; set; }

		/// <summary>
		/// Model de validation pour un nouveau compte.
		/// </summary>
		CompteValidation CompteValidation { get; set; }

		/// <summary>
		/// Référence au tableau Radzen
		/// </summary>
		RadzenGrid<Compte> CompteGrid { get; set; }

		/// <summary>
		/// Indicateur si les données sont chargées.
		/// </summary>
		bool IsLoaded { get; set; }

		/// <summary>
		/// Indicateur s'il faut ouvrir le Dialog
		/// </summary>
		bool DialogIsOpenNewAccount { get; set; }

		/// <summary>
		/// Charge tous les comptes.
		/// </summary>
		/// <returns></returns>
		Task LoadAllAccounts();

		/// <summary>
		/// Ouvre le menu pour avoir un nouveau compte
		/// </summary>
		void OpenNewAccount();

		/// <summary>
		/// Méthode pour valider le nouveau compte
		/// </summary>
		void OnValidSubmit();

		/// <summary>
		/// Ferme la fenêtre d'un nouveau compte
		/// </summary>
		void CloseNewAccount();

		/// <summary>
		/// Permet au ViewModel d'indiquer un changement d'état à la vue.
		/// </summary>
		/// <param name="stateHasChange"></param>
		void SetStateHasChanged(Action stateHasChange);
	}
}
