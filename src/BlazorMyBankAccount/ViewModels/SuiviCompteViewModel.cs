namespace BlazorMyBankAccount.ViewModels
{
    public class SuiviCompteViewModel : BaseViewModel, ISuiviCompteViewModel
    {
        private ISuiviCompteData dataContext;

        public SuiviCompteViewModel(ISuiviCompteData data, NotificationService notificationService)
            : base(notificationService)
        {
            dataContext = data;
            TransacValidation = new TransactionValidation();
        }


        #region Implement ISuiviCompte

        public IEnumerable<Compte> Comptes { get; private set; }

        public List<Suivicompte> SuiviDuCompte { get; private set; }

        public IEnumerable<Typestransaction> TypesTransaction { get; private set;  }

        public TransactionValidation TransacValidation { get; private set;  }

        public bool IsLoading { get; private set; }

        
        public RadzenGrid<Suivicompte> SaisieGrid { get; set; }



        private Compte CompteSelected;

        public async Task InitData()
        {
            IsLoading = true;
            TypesTransaction = await dataContext.GetTypesTransaction();
            Comptes = await dataContext.GetComptes();
            SuiviDuCompte = await dataContext.GetSuivicomptes(0, 2022, 01);
            IsLoading = false;
            CompteSelected = await dataContext.GetCompte(0);
        }


        public async Task OnSelectCompte(object compte)
        {
            IsLoading = true;
            Compte compteSelected = (Compte)compte;
            CompteSelected = compteSelected;

            SuiviDuCompte = await dataContext.GetSuivicomptes(compteSelected.Idcompte, 2022, 01);
            IsLoading = false;
        }


        public async void OnValidSubmit()
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
                    Idannee = 2022,
                    Idmois = 1
                };
                await dataContext.AddNouvelleSaisie(nouvelleEntre);

                SuiviDuCompte.Add(nouvelleEntre);
                await SaisieGrid.Reload();

                //string message = $"Nouveau compte : {compte.Nomcompte} ajouté";
                NotificationSuccess("Ajout OK", "Pas planté");
                //Log.Information("COMPTE - " + message);
                TransacValidation = new TransactionValidation();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "SuiviCompteViewModel - OnValidSubmit");
                NotificationError("Saisie non créée");
            }
        }

        public void AnnulerSaisie()
        {
            TransacValidation = new TransactionValidation();
        }

        #endregion

    }
}
