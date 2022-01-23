namespace BlazorMyBankAccount.ViewModels.Configuration
{
    public class BudgetConfigViewModel : BaseViewModel, IBudgetConfig
    {
        /// <inheritdoc cref="IBudgetConfig.AllBudgets"/>
        public List<BudgetCompteView> AllBudgets { get; set; }

        /// <inheritdoc cref="IBudgetConfig.AllTypesBudget"/>
        public List<Typebudget> AllTypesBudget { get; set; }

        /// <inheritdoc cref="IBudgetConfig.AllComptes"/>
        public List<Compte> AllComptes { get; set; }

        /// <inheritdoc cref="IBudgetConfig.BudgetValidation"/>
        public BudgetValidation BudgetValidation { get; set; }

        /// <inheritdoc cref="IBudgetConfig.BudgetGrid"/>
        public RadzenGrid<BudgetCompteView> BudgetGrid { get; set; }

        /// <inheritdoc cref="IBudgetConfig.IsLoaded"/>
        public bool IsLoaded { get; set; }

        /// <inheritdoc cref="IBudgetConfig.DialogIsOpenNewBudget"/>
        public bool DialogIsOpenNewBudget { get; set; }


        private IBudgetBanking dataAccess;
        private Action StateChange;

        public BudgetConfigViewModel(IBudgetBanking data, NotificationService notificationService)
            : base(notificationService)
        {
            dataAccess = data;

            IsLoaded = false;
            BudgetValidation = new BudgetValidation();
            AllBudgets = new List<BudgetCompteView>();
        }

        /// <inheritdoc cref="IBudgetConfig.CloseNewBudget"/>
        public void CloseNewBudget()
        {
            DialogIsOpenNewBudget = false;
            BudgetValidation = new BudgetValidation();
            StateChange.Invoke();
        }

        /// <inheritdoc cref="IBudgetConfig.LoadAllBudgets"/>
        public async Task LoadAllBudgets()
        {
            AllTypesBudget = await dataAccess.GetTypesBudget();
            var budgets = await dataAccess.GetBudgets();
            AllComptes = await dataAccess.GetAccounts();
            IsLoaded = true;

            foreach (var item in budgets)
            {
                BudgetCompteView view = new BudgetCompteView();
                view.Budget = item;
                view.Compte = item.Idcomptes.FirstOrDefault();

                AllBudgets.Add(view);
            }
        }

        /// <inheritdoc cref="IBudgetConfig.OnValidSubmit"/>
        public async void OnValidSubmit()
        {
            try
            {
                // Ajout dans la base de donnée.
                Budget newBudget = await dataAccess.AddBudget(BudgetValidation.NomBudget, BudgetValidation.Description, BudgetValidation.CompteId, BudgetValidation.TypeBudgetId, BudgetValidation.Montant);

                AllBudgets.Add(newBudget.ToBudgetCompteView());
                await BudgetGrid.Reload();

                string message = $"Nouveau Budget : {newBudget.Nombudget} ajouté";
                NotificationSuccess("Sauvegarde OK", message);
                Log.Information("BUDGET - " + message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "BudgetConfigViewModel - OnValidSubmit");
                NotificationError("Budget non créé");
            }

            CloseNewBudget();
        }

        /// <inheritdoc cref="IBudgetConfig.OpenNewBudget"/>
        public void OpenNewBudget()
        {
            DialogIsOpenNewBudget = true;
        }

        /// <inheritdoc cref="IBudgetConfig.SetStateHasChanged(Action)"/>
        public void SetStateHasChanged(Action stateHasChange)
        {
            StateChange = stateHasChange;
        }

        /// <inheritdoc cref="IBudgetConfig.OnSelectCompte(object)"/>
        public void OnSelectCompte(object compteSelected)
        {
            BudgetValidation.CompteId = (Compte)compteSelected;
        }

        /// <inheritdoc cref="IBudgetConfig.OnSelectTypeBudget(object)"/>
        public void OnSelectTypeBudget(object typeSelected)
        {
            BudgetValidation.TypeBudgetId = (Typebudget)typeSelected;
        }

        /// <inheritdoc cref="IBudgetConfig.OnChangeMontant(decimal)"/>
        public void OnChangeMontant(decimal montant)
        {
            BudgetValidation.Montant = montant;
        }
    }

}
