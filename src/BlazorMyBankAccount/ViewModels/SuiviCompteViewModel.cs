namespace BlazorMyBankAccount.ViewModels
{
    public class SuiviCompteViewModel : BaseViewModel, ISuiviCompteViewModel
    {
        private ISuiviCompteData dataContext;
        private TransactionValidation TransacValidation;
        private VirementOperationValidation VirementOperationValidation;

        public SuiviCompteViewModel(ISuiviCompteData data, NotificationService notificationService)
            : base(notificationService)
        {
            dataContext = data;
            TransacValidation = new TransactionValidation();
            VirementOperationValidation = new VirementOperationValidation();
        }


        #region Implement ISuiviCompte

        public IEnumerable<Compte> Comptes { get; private set; }

        public List<Suivicompte> SuiviDuCompte { get; private set; }

        public RenderFragment DisplayRenderFragment { get; private set; }

        public IEnumerable<Typestransaction> TypesTransaction { get; private set;  }

        

        public bool IsLoading { get; private set; }

        public bool HasConfig { get; private set; }

        public IEnumerable<Budget> Budgets { get; private set; }


        public RadzenGrid<Suivicompte> SaisieGrid { get; set; }
        public Compte CompteSelected { get; set; }



        private Configbank Configbank;
        private Action StateChanged;

        public async Task InitData()
        {
            IsLoading = true;
            TypesTransaction = await dataContext.GetTypesTransaction();
            Comptes = await dataContext.GetComptes();
            
            Configbank = await dataContext.GetConfig();
            if(Configbank != null)
            {
                CompteSelected = Configbank.IdcomptedefaultNavigation;
                SuiviDuCompte = await dataContext.GetSuivicomptes(CompteSelected.Idcompte, Configbank.Annee, Configbank.Mois);
                Budgets = await dataContext.GetBudgets(CompteSelected.Idcompte);
                HasConfig = true;
            }
            else
            {
                HasConfig = false;
                Budgets = new List<Budget>();
            }
            
            IsLoading = false;
        }


        public async Task OnSelectCompte(object compte)
        {
            IsLoading = true;
            Compte compteSelected = (Compte)compte;
            CompteSelected = compteSelected;

            SuiviDuCompte = await dataContext.GetSuivicomptes(compteSelected.Idcompte, Configbank.Annee, Configbank.Mois);
            Budgets = await dataContext.GetBudgets(CompteSelected.Idcompte);

            IsLoading = false;
        }

        public void SetStateHasChanged(Action state)
		{
            StateChanged = state;

        }



		#region Affichage nouvelle opération
        

		public void DisplayNewOperation()
        {
            RenderFragment CreateCompo() => builder =>
            {
                builder.OpenComponent(0, typeof(NouvelleOperation));
                
                builder.AddAttribute(1, "ModelValidation", this.TransacValidation);
                builder.AddAttribute(2, "Transactions", this.TypesTransaction);
                builder.AddAttribute(3, "Budgets", Budgets);

                EventCallback eventOnValide = EventCallback.Factory.Create(this, OnValidSubmit);
                builder.AddAttribute(4, "OnValidSubmit", eventOnValide);

                EventCallback eventOnCancel = EventCallback.Factory.Create(this, AnnulerSaisie);
                builder.AddAttribute(5, "AnnuleSaisie", eventOnCancel);

                builder.CloseComponent();
            };

            DisplayRenderFragment = CreateCompo();
        }


        private async void OnValidSubmit()
        {
            try
            {
                Suivicompte nouvelleEntre = new Suivicompte()
                {
                    Datetransaction = TransacValidation.DateTransaction,
                    Nomtransaction = TransacValidation.NomTransaction,
                    Montant = TransacValidation.Montant,
                    Isvalidate = TransacValidation.IsValide,
                    Type = TransacValidation.TypeTransac,
                    Idcompte = CompteSelected.Idcompte,
                    Idannee = Configbank.Annee,
                    Idmois = Configbank.Mois
                };

                if(TransacValidation.Budget != null)
				{
                    nouvelleEntre.Idbudget = TransacValidation.Budget.Idbudget;
                }

                await dataContext.AddNouvelleSaisie(nouvelleEntre);

                SuiviDuCompte.Add(nouvelleEntre);
                await SaisieGrid.Reload();

                NotificationSuccess("Ajout OK", "Ajout de l'opération");
                TransacValidation = new TransactionValidation();
                StateChanged.Invoke();
            }
            catch (Exception ex)
            {
                ReportError(ex, "SuiviCompteViewModel - OnValidSubmit", "Saisie non créée");
            }
        }

        private void AnnulerSaisie()
        {
            DisplayRenderFragment = null;
            TransacValidation = new TransactionValidation();
            StateChanged.Invoke();
        }

        #endregion

        #region Affichage virement

        private IEnumerable<Budget> budgetsOnCompte = new List<Budget>();

        public void DisplayVirement()
		{
            var othersComptes = Comptes.ToList();
            othersComptes.Remove(CompteSelected);

            RenderFragment CreateCompo() => builder =>
            {
                builder.OpenComponent(0, typeof(Virement));
                
                builder.AddAttribute(1, "ModelValidation", this.VirementOperationValidation);
                builder.AddAttribute(2, "Comptes", othersComptes);
                builder.AddAttribute(3, "Budgets", budgetsOnCompte);

                EventCallback eventOnValide = EventCallback.Factory.Create(this, OnValidSubmitVirement);
                builder.AddAttribute(4, "OnValidSubmit", eventOnValide);

                EventCallback eventOnCancel = EventCallback.Factory.Create(this, AnnulerVirement);
                builder.AddAttribute(5, "AnnulerVirement", eventOnCancel);

                var eventOnSelectCompte = EventCallback.Factory.Create(this, OnChangeCompteVirement);
                builder.AddAttribute(6, "OnSelectCompte", eventOnSelectCompte);

                builder.CloseComponent();
            };

            DisplayRenderFragment = CreateCompo();
        }

        private async void OnChangeCompteVirement(object compte)
		{
            if (compte == null)
            {
                budgetsOnCompte = new List<Budget>();
                StateChanged.Invoke();
                return;
            }

            Compte compteSelected = (Compte)compte;
            budgetsOnCompte = await dataContext.GetBudgets(compteSelected.Idcompte);
            StateChanged.Invoke();
        }

        private async void OnValidSubmitVirement()
		{
            try
            {
                // Ligne pour le compte emetteur.
                Suivicompte compteEmetteur = new Suivicompte()
                {
                    Datetransaction = VirementOperationValidation.DateTransaction,
                    Nomtransaction = VirementOperationValidation.NomTransaction,
                    Montant = decimal.Negate(VirementOperationValidation.Montant),
                    Isvalidate = true, // toujours validé
                    Typeid = 2, // Virement
                    Idcompte = CompteSelected.Idcompte,
                    Idannee = Configbank.Annee,
                    Idmois = Configbank.Mois
                };

                // Ligne pour le compte receveur.
                Suivicompte compteReceveur = new Suivicompte()
                {
                    Datetransaction = VirementOperationValidation.DateTransaction,
                    Nomtransaction = VirementOperationValidation.NomTransaction,
                    Montant = VirementOperationValidation.Montant,
                    Isvalidate = true, // toujours validé
                    Typeid = 2, // Virement
                    Idcompte = VirementOperationValidation.CompteArrive.Idcompte,
                    Idannee = Configbank.Annee,
                    Idmois = Configbank.Mois
                };
				if (VirementOperationValidation.Budget != null)
				{
                    compteReceveur.Idbudget = VirementOperationValidation.Budget.Idbudget;
				}

				await dataContext.AddVirement(compteEmetteur, compteReceveur);

                SuiviDuCompte.Add(compteEmetteur);
				await SaisieGrid.Reload();

                NotificationSuccess("Ajout OK", "Ajout de l'opération");
                TransacValidation = new TransactionValidation();
                StateChanged.Invoke();
            }
            catch (Exception ex)
            {
                ReportError(ex, "SuiviCompteViewModel - OnValidSubmit", "Saisie non créée");
            }
        }

        private void AnnulerVirement()
        {
            DisplayRenderFragment = null;
            VirementOperationValidation = new VirementOperationValidation();
            StateChanged.Invoke();
        }

        #endregion


        #endregion

    }
}
