
namespace BlazorMyBankAccount.ViewModels.Configuration
{
    public class CompteConfigViewModel : BaseViewModel, ICompteConfig
    {
        /// <inheritdoc cref="ICompteConfig.AllAccounts"/>
        public List<Compte> AllAccounts { get; set; }

        /// <inheritdoc cref="ICompteConfig.IsLoaded"/>
        public bool IsLoaded { get; set; }

        /// <inheritdoc cref="ICompteConfig.DialogIsOpenNewAccount"/>
        public bool DialogIsOpenNewAccount { get; set; }

        /// <inheritdoc cref="ICompteConfig.CompteValidation"/>
        public CompteValidation CompteValidation { get; set; }

        /// <inheritdoc cref="ICompteConfig.CompteGrid"/>
        public RadzenGrid<Compte> CompteGrid { get; set; }

        private ICompteBanking dataAccess;
        private Action StateChange;

        public CompteConfigViewModel(ICompteBanking data, NotificationService notificationService)
            : base(notificationService)
        {
            dataAccess = data;

            IsLoaded = false;
            CompteValidation = new CompteValidation();
        }

        /// <inheritdoc cref="ICompteConfig.LoadAllAccounts"/>
        public async Task LoadAllAccounts()
        {
            AllAccounts = await dataAccess.GetAccounts();
            IsLoaded = true;
        }

        /// <inheritdoc cref="ICompteConfig.OpenNewAccount"/>
        public void OpenNewAccount()
        {
            DialogIsOpenNewAccount = true;
        }

        /// <inheritdoc cref="ICompteConfig.OnValidSubmit"/>
        public async void OnValidSubmit()
        {
			try
			{
				// Ajout dans la base de donnée.
				Compte compte = new Compte()
				{
					Nomcompte = CompteValidation.NomCompte,
					Description = CompteValidation.Description
				};
				await dataAccess.AddAccount(compte);

				AllAccounts.Add(compte);
                await CompteGrid.Reload();

                string message = $"Nouveau compte : {compte.Nomcompte} ajouté";
                NotificationSuccess("Sauvegarde OK", message);
				Log.Information("COMPTE - " + message);

            }
			catch (Exception ex)
			{
				Log.Error(ex, "CompteConfigViewModel - OnValidSubmit");
                NotificationError("Compte non créé");
            }

            CloseNewAccount();
		}

        /// <inheritdoc cref="ICompteConfig.CloseNewAccount"/>
        public void CloseNewAccount()
        {
            DialogIsOpenNewAccount = false;
            CompteValidation = new CompteValidation();
            StateChange.Invoke();
        }

        /// <inheritdoc cref="ICompteConfig.SetStateHasChanged(Action)"/>
        public void SetStateHasChanged(Action stateHasChange)
        {
            StateChange = stateHasChange;
        }
    }
}
