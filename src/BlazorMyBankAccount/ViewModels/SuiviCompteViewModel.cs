﻿namespace BlazorMyBankAccount.ViewModels
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

        public bool HasConfig { get; private set; }


        public RadzenGrid<Suivicompte> SaisieGrid { get; set; }
        public Compte CompteSelected { get; set; }



        private Configbank Configbank;

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
                HasConfig = true;
            }
            else
            {
                HasConfig = false;
            }
            
            IsLoading = false;
        }


        public async Task OnSelectCompte(object compte)
        {
            IsLoading = true;
            Compte compteSelected = (Compte)compte;
            CompteSelected = compteSelected;

            SuiviDuCompte = await dataContext.GetSuivicomptes(compteSelected.Idcompte, Configbank.Annee, Configbank.Mois);
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
                    Idannee = Configbank.Annee,
                    Idmois = Configbank.Mois
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
