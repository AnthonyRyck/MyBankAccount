using BlazorMyBankAccount.Data.DataView;

namespace BlazorMyBankAccount.ViewModels.Configuration
{
    public interface IBudgetConfig
    {
		/// <summary>
		/// Tous les Budgets
		/// </summary>
		List<BudgetCompteView> AllBudgets { get; set; }

		/// <summary>
		/// Tous les comptes
		/// </summary>
		List<Compte> AllComptes{ get; set; }

		/// <summary>
		/// Model de validation pour un nouveau budget.
		/// </summary>
		BudgetValidation BudgetValidation { get; set; }

		/// <summary>
		/// Référence au tableau Radzen
		/// </summary>
		RadzenGrid<BudgetCompteView> BudgetGrid { get; set; }

		/// <summary>
		/// Indicateur si les données sont chargées.
		/// </summary>
		bool IsLoaded { get; set; }

		/// <summary>
		/// Indicateur s'il faut ouvrir le Dialog
		/// </summary>
		bool DialogIsOpenNewBudget { get; set; }

		/// <summary>
		/// Charge tous les Budgets.
		/// </summary>
		/// <returns></returns>
		Task LoadAllBudgets();

		/// <summary>
		/// Ouvre le menu pour avoir un nouveau Budget
		/// </summary>
		void OpenNewBudget();

		/// <summary>
		/// Méthode pour valider le nouveau Budget
		/// </summary>
		void OnValidSubmit();

		/// <summary>
		/// Ferme la fenêtre d'un nouveau Budget
		/// </summary>
		void CloseNewBudget();

		/// <summary>
		/// Permet au ViewModel d'indiquer un changement d'état à la vue.
		/// </summary>
		/// <param name="stateHasChange"></param>
		void SetStateHasChanged(Action stateHasChange);


		void OnSelectCompte(object compteSelected);
	}
}
