namespace BlazorMyBankAccount.ViewModels
{
    public class HomePageViewModel : BaseViewModel, IHomePage
    {
        private IConfigBanking dataContext;

        private Configbank configbank;

        public HomePageViewModel(NotificationService notificationService, IConfigBanking data) 
            : base(notificationService)
        {
            IsLoaded = false;
            dataContext = data;
        }


        public bool IsLoaded { get; private set; }

        public bool HasConfig { get; private set; }

        public async Task InitData()
        {
            configbank = await dataContext.GetConfiguration();
            HasConfig = (configbank != null);
            IsLoaded = true;
        }
    }
}
