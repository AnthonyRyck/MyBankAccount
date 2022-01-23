namespace BlazorMyBankAccount.ViewModels.Configuration
{
    public interface IConfigTransaction
    {
		/// <summary>
		/// Toutes les transactions obligatoires.
		/// </summary>
		List<Transactionobligatoire> AllTransactions { get; }

		/// <summary>
		/// Liste de tous les comptes
		/// </summary>
		IEnumerable<Compte> AllComptes { get; }

		/// <summary>
		/// Liste de tous les types de transaction
		/// </summary>
		IEnumerable<Typestransaction> AllTypes { get; }

		/// <summary>
		/// Indicateur si les données sont chargées.
		/// </summary>
		bool IsLoaded { get; }

		/// <summary>
		/// Modele de validation d'une saisie.
		/// </summary>
		TransactionMensuelleValidation ModelValidation { get; }

		/// <summary>
		/// Indicateur s'il faut ouvrir le Dialog
		/// </summary>
		bool DialogIsOpen { get; }

		/// <summary>
		/// Référence au tableau Radzen
		/// </summary>
		RadzenGrid<Transactionobligatoire> TransacObligatoireGrid { get; set; }

		/// <summary>
		/// Charge toutes les transactions.
		/// </summary>
		/// <returns></returns>
		Task LoadAllTransactions();

		/// <summary>
		/// Ouvre le menu pour avoir une nouvelle transaction
		/// </summary>
		void OpenNewAccount();

		/// <summary>
		/// Méthode pour valider la nouvelle transaction
		/// </summary>
		void OnValidSubmit();

		/// <summary>
		/// Ferme la fenêtre d'une nouvelle transaction.
		/// </summary>
		void CloseNewAccount();
	}
}
