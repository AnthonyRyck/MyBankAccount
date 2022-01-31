namespace BlazorMyBankAccount.ViewModels.Configuration
{
    public class AppConfigViewModel : BaseViewModel, IAppConfig
    {
        private readonly IConfigBanking _contextData;
        private Action StateView;

        public AppConfigViewModel(NotificationService notificationService, IConfigBanking configBanking) 
            : base(notificationService)
        {
            _contextData = configBanking;
            DialogIsOpen = false;
            ValidationModel = new ConfigValidation();
        }

        /// <inheritdoc cref="IAppConfig.IsLoaded"/>
        public bool IsLoaded { get; private set; }

        /// <inheritdoc cref="IAppConfig.Comptes"/>
        public IEnumerable<Compte> Comptes { get; private set; }

        /// <inheritdoc cref="IAppConfig.ValidationModel"/>
        public ConfigValidation ValidationModel { get; private set; }

        /// <inheritdoc cref="IAppConfig.DialogIsOpen"/>
        public bool DialogIsOpen { get; private set; }

        /// <inheritdoc cref="IAppConfig.LaConfiguration"/>
        public Configbank LaConfiguration { get; private set; }

        /// <inheritdoc cref="IAppConfig.InitData"/>
        public async Task InitData()
        {
            IsLoaded = false;
            LaConfiguration = await _contextData.GetConfiguration();
            if(LaConfiguration != null)
            {
                ValidationModel.Annee = LaConfiguration.Annee;
                ValidationModel.Mois = LaConfiguration.Mois;
                ValidationModel.CompteParDefault = LaConfiguration.IdcomptedefaultNavigation;
            }

            Comptes = await _contextData.GetComptes();
            IsLoaded = true;
        }

        /// <inheritdoc cref="IAppConfig.SetStateHasChanged(Action)"/>
        public void SetStateHasChanged(Action state)
        {
            StateView = state;
        }

        /// <inheritdoc cref="IAppConfig.OnCancel"/>
        public void OnCancel()
        {
            DialogIsOpen = false;
            if (LaConfiguration != null)
            {
                ValidationModel.Annee = LaConfiguration.Annee;
                ValidationModel.Mois = LaConfiguration.Mois;
                ValidationModel.CompteParDefault = LaConfiguration.IdcomptedefaultNavigation;
            }
        }

        /// <inheritdoc cref="IAppConfig.OnValidSubmit"/>
        public async void OnValidSubmit()
        {
            try
            {
                LaConfiguration = await _contextData.AddOrUpdate(ValidationModel.CompteParDefault, ValidationModel.Annee, ValidationModel.Mois);
                DialogIsOpen = false;

                ValidationModel.Annee = LaConfiguration.Annee;
                ValidationModel.Mois = LaConfiguration.Mois;
                ValidationModel.CompteParDefault = LaConfiguration.IdcomptedefaultNavigation;

                StateView.Invoke();

                this.NotificationSuccess("Sauvegarde OK", "La configuration est sauvegardé");
            }
            catch (Exception ex)
            {
                this.ReportError(ex, "Erreur sur AppConfigViewModel - OnValidSubmit", "Configuration non mis à jour.");
            }
        }

        /// <inheritdoc cref="IAppConfig.OpenConfig"/>
        public void OpenConfig()
        {
            DialogIsOpen = true;
        }

        public string GetMois()
        {
            return LaConfiguration.GetMois();
        }

        /// <inheritdoc cref="IAppConfig.OnClickChangeMois"/>
        public async Task OnClickChangeMois()
        {
            int nouveauMois = LaConfiguration.Mois;
            int nouvelleAnnee = LaConfiguration.Annee;

            // Changement de mois et/ou d'annee
            if (nouveauMois == 12)
            {
                nouveauMois = 1;
                nouvelleAnnee++;
            }
            else
            {
                nouveauMois++;
            }

            LaConfiguration = await _contextData.AddOrUpdate(LaConfiguration.IdcomptedefaultNavigation, nouvelleAnnee, nouveauMois);

            // Copie des transactions obligatoires dans la table suivie.
            await _contextData.CreateNewMonth(LaConfiguration.Idcomptedefault, LaConfiguration.Annee, LaConfiguration.Mois);
        }
    }
}
