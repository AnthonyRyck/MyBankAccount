using Microsoft.AspNetCore.Components.Web;

namespace BlazorMyBankAccount.ViewModels
{
    public class SuiviCompteViewModel : BaseViewModel, ISuiviCompteViewModel
    {
        private ISuiviCompteData dataContext;
        private TransactionValidation TransacValidation;
        private VirementOperationValidation VirementOperationValidation;
        private ContextMenuService ContextMenu;

        private const string MENU_NEW_TRANSAC_OBLI = "Ajouter aux opérations répétées";
        private const string MENU_VALIDER = "Valider l'opération";
        private const string MENU_INVALIDER = "Invalider l'opération";

        public SuiviCompteViewModel(ISuiviCompteData data, NotificationService notificationService, ContextMenuService contextMenuSvc)
            : base(notificationService)
        {
            ContextMenu = contextMenuSvc;
            dataContext = data;
            TransacValidation = new TransactionValidation();
            VirementOperationValidation = new VirementOperationValidation();
        }


        #region Implement ISuiviCompte

        public IEnumerable<Compte> Comptes { get; private set; }

        public decimal MontantPrevisionnel { get; private set; }

        public decimal MontantActuel { get; private set; }

        public List<Suivicompte> SuiviDuCompte { get; private set; }

        public RenderFragment DisplayRenderFragment { get; private set; }

        public IEnumerable<Typestransaction> TypesTransaction { get; private set;  }

        public bool IsLoading { get; private set; }

        public bool HasConfig { get; private set; }

        public IEnumerable<Budget> Budgets { get; private set; }

        public decimal MontantRestantBudget { get; private set; }

        public RadzenDataGrid<Suivicompte> SaisieGrid { get; set; }
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

                await FaireSomme();

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

            await FaireSomme();

            IsLoading = false;
        }

        public void SetStateHasChanged(Action state)
		{
            StateChanged = state;

        }

        public string GetMois()
        {
            return Configbank.GetMois();
        }

        #region ContextMenu

        public void OnCellContextMenu(DataGridCellMouseEventArgs<Suivicompte> args)
        {
            var menus = new List<ContextMenuItem> {
                new ContextMenuItem(){ Text = MENU_NEW_TRANSAC_OBLI, Value = args }
                };

            if (args.Data.Isvalidate)
                menus.Add(new ContextMenuItem() { Text = MENU_INVALIDER, Value = args });
            else
                menus.Add(new ContextMenuItem() { Text = MENU_VALIDER, Value = args });

            ContextMenu.Open(args, menus, OnMenuItemClick);
        }

        private async void OnMenuItemClick(MenuItemEventArgs args)
        {
            var ligneSuivi = (DataGridCellMouseEventArgs<Suivicompte>)args.Value;

            switch (args.Text)
            {
                case MENU_NEW_TRANSAC_OBLI:
                    if (!ligneSuivi.Data.Isvalidate)
                    {
                        NotificationError("Ne peut être que sur une ligne validée.");
                        return;
                    }
                    await AddNewTransactionObligatoire(ligneSuivi.Data);
                    break;

                case MENU_VALIDER:
                    await ValidateTransaction(ligneSuivi.Data, true);
                    await SaisieGrid.Reload();
                    break;

                case MENU_INVALIDER:
                    await ValidateTransaction(ligneSuivi.Data, false);
                    await SaisieGrid.Reload();
                    break;

                default:
                    break;
            }

            ContextMenu.Close();
        }

        private async Task ValidateTransaction(Suivicompte ligneSuivi, bool canValidate)
        {
            await dataContext.Validate(ligneSuivi, canValidate);
            await FaireSomme();
            StateChanged.Invoke();
        }

        private async Task AddNewTransactionObligatoire(Suivicompte ligneCompte)
        {
            Transactionobligatoire newTransac = new Transactionobligatoire()
            {
                Idcompte = ligneCompte.Idcompte,
                Jour = ligneCompte.Datetransaction.Value.Day,
                Montant = ligneCompte.Montant,
                Nomtransaction = ligneCompte.Nomtransaction,
                Commentaire = ligneCompte.Commentaire,
                Type = ligneCompte.Type
            };

            await dataContext.AddTransactionObligatoire(newTransac);
        }

        #endregion

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
                    Idmois = Configbank.Mois,
                    Commentaire = TransacValidation.Commentaire
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
                await FaireSomme();
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
            await FaireSomme();
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

        #region Edit ligne

        public bool IsRowEdit { get; private set; }
        private Suivicompte ligneBackup;

        public async Task EditRowSuivi(Suivicompte suivicompte)
        {
            IsRowEdit = true;
            ligneBackup = ToBackup(suivicompte);
            await SaisieGrid.EditRow(suivicompte);
        }

        public async Task SaveRow(Suivicompte suivicompte)
        {
            await dataContext.UpdateLigne(suivicompte);
            await SaisieGrid.UpdateRow(suivicompte);

            // Si c'est un budget
            if(suivicompte.Idbudget != null)
            {
                // update les sommes.
                Budget budgetSelected = Budgets.FirstOrDefault(x => x.Idbudget == suivicompte.Idbudget);
                
                if (budgetSelected.Typebudget.Nomtypebudget == "Prévision dépense")
                {
                    MontantRestantBudget = budgetSelected.Montant.Value + SuiviDuCompte.Sum(x => x.Montant);
                }
            }
            
            await FaireSomme();
            IsRowEdit = false;
        }

        public void CancelEdit(Suivicompte suivicompte)
        {
            SaisieGrid.CancelEditRow(ReverseTo(suivicompte));
            IsRowEdit = false;
        }

        #endregion


        public async Task LoadBudget(int num)
        {
            if (num == 0)
            {
                SuiviDuCompte = await dataContext.GetSuivicomptes(CompteSelected.Idcompte, Configbank.Annee, Configbank.Mois);
                await FaireSomme();
            }
            else
            {
                Budget budgetSelected = Budgets.ToList()[num - 1];
                SuiviDuCompte = await dataContext.GetSuivicomptes(CompteSelected.Idcompte, Configbank.Annee, Configbank.Mois, budgetSelected.Idbudget);

                if(budgetSelected.Typebudget.Nomtypebudget == "Prévision dépense")
                {
                    MontantRestantBudget = budgetSelected.Montant.Value + SuiviDuCompte.Sum(x => x.Montant);
                }
            }
        }

        #endregion

        private async Task FaireSomme()
        {
            // somme des budgets "depenses"
            decimal montantsBudgetsPrev = Budgets.Sum(x => x.Montant) ?? decimal.Zero;
            decimal montantBudgetsValide = await dataContext.GetMontantBudgetValide(CompteSelected.Idcompte, Configbank.Annee, Configbank.Mois);

            MontantActuel = SuiviDuCompte.Where(x => x.Isvalidate).Sum(x => x.Montant) + montantBudgetsValide;

            MontantPrevisionnel = MontantActuel + SuiviDuCompte.Where(x => !x.Isvalidate).Sum(x => x.Montant) + decimal.Negate(montantsBudgetsPrev);
        }

        private Suivicompte ToBackup(Suivicompte suivicompte)
        {
            return new Suivicompte()
            {
                Commentaire = suivicompte.Commentaire,
                Datetransaction = suivicompte.Datetransaction,
                Idannee = suivicompte.Idannee,
                IdanneeNavigation = suivicompte.IdanneeNavigation,
                Idbudget = suivicompte.Idbudget,
                Idcompte = suivicompte.Idcompte,
                IdcompteNavigation = suivicompte.IdcompteNavigation,
                Idmois = suivicompte.Idmois,
                IdmoisNavigation = suivicompte.IdmoisNavigation,
                Idsuivi = suivicompte.Idsuivi,
                Isvalidate = suivicompte.Isvalidate,
                Montant = suivicompte.Montant,
                Nomtransaction = suivicompte.Nomtransaction,
                Type = suivicompte.Type,
                Typeid = suivicompte.Typeid
            };
        }

        private Suivicompte ReverseTo(Suivicompte suivicompte)
        {
            suivicompte.Commentaire = ligneBackup.Commentaire;
            suivicompte.Datetransaction = ligneBackup.Datetransaction;
            suivicompte.Idannee = ligneBackup.Idannee;
            suivicompte.IdanneeNavigation = ligneBackup.IdanneeNavigation;
            suivicompte.Idbudget = ligneBackup.Idbudget;
            suivicompte.Idcompte = ligneBackup.Idcompte;
            suivicompte.IdcompteNavigation = ligneBackup.IdcompteNavigation;
            suivicompte.Idmois = ligneBackup.Idmois;
            suivicompte.IdmoisNavigation = ligneBackup.IdmoisNavigation;
            suivicompte.Idsuivi = ligneBackup.Idsuivi;
            suivicompte.Isvalidate = ligneBackup.Isvalidate;
            suivicompte.Montant = ligneBackup.Montant;
            suivicompte.Nomtransaction = ligneBackup.Nomtransaction;
            suivicompte.Type = ligneBackup.Type;
            suivicompte.Typeid = ligneBackup.Typeid;

            return suivicompte;
        }

    }
}
