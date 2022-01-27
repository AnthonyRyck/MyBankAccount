namespace BlazorMyBankAccount.ViewModels.Configuration
{
    public class ConfigTransactionViewModel : BaseViewModel, IConfigTransaction
    {
        private ITransactionData dataAccess;
        private Action stateChanged;

        public ConfigTransactionViewModel(ITransactionData context, NotificationService notificationService)
            : base(notificationService)
        {
            dataAccess = context;
            ModelValidation = new TransactionMensuelleValidation();
        }


        #region Implement IConfigTransaction


        /// <inheritdoc cref="IConfigTransaction.AllTransactions"/>
        public List<Transactionobligatoire> AllTransactions { get; private set; }

        /// <inheritdoc cref="IConfigTransaction.AllComptes"/>
		public IEnumerable<Compte> AllComptes { get; private set; }

        /// <inheritdoc cref="IConfigTransaction.AllTypes"/>
        public IEnumerable<Typestransaction> AllTypes { get; private set; }

        /// <inheritdoc cref="IConfigTransaction.IsLoaded"/>
        public bool IsLoaded { get; private set; }

        /// <inheritdoc cref="IConfigTransaction.ModelValidation"/>
        public TransactionMensuelleValidation ModelValidation { get; private set; }

        /// <inheritdoc cref="IConfigTransaction.DialogIsOpen"/>
        public bool DialogIsOpen { get; private set; }

        /// <inheritdoc cref="IConfigTransaction.TransacObligatoireGrid"/>
        public RadzenGrid<Transactionobligatoire> TransacObligatoireGrid { get; set; }


        /// <inheritdoc cref="IConfigTransaction.LoadAllTransactions"/>
        public async Task LoadAllTransactions()
        {
            try
            {
                IsLoaded = false;
                AllTransactions = await dataAccess.GetTransactions();
                AllComptes = await dataAccess.GetAllComptes();
                AllTypes = await dataAccess.GetAllTypes();
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                ReportError(ex, "ConfigTransactionViewModel - LoadAllTransactions", "Erreur chargement transactions");
            }
        }

        /// <inheritdoc cref="IConfigTransaction.OnValidSubmit"/>
        public async void OnValidSubmit()
        {
            try
            {
                var nouvelleTransaction = await dataAccess.AddTransaction(ModelValidation.NomTransaction, ModelValidation.Montant,
                    ModelValidation.Jour, ModelValidation.CompteSelected.Idcompte, ModelValidation.Typestransaction);

                AllTransactions.Add(nouvelleTransaction);
                await TransacObligatoireGrid.Reload();

                string message = $"Nouvelle operation : {ModelValidation.NomTransaction} ajoutée";
                NotificationSuccess("Sauvegarde OK", message);

                CloseNewAccount();
                stateChanged?.Invoke();
            }
            catch (Exception ex)
            {
                ReportError(ex, "ConfigTransactionViewModel - OnValidSubmit", "Transaction non enregistré");
            }
        }

        /// <inheritdoc cref="IConfigTransaction.OpenNewAccount"/>
        public void OpenNewAccount()
        {
            DialogIsOpen = true;
        }

        /// <inheritdoc cref="IConfigTransaction.CloseNewAccount"/>
        public void CloseNewAccount()
        {
            DialogIsOpen = false;
            ModelValidation = new TransactionMensuelleValidation();
        }

        public void SetStateHasChanged(Action stateHasChanged)
        {
            stateChanged = stateHasChanged;
        }

        #endregion

    }
}
