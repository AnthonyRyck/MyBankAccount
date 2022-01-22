namespace BlazorMyBankAccount.ViewModels
{
    public abstract class BaseViewModel
    {
        private NotificationService NotifSvc;

        public BaseViewModel(NotificationService notificationService)
        {
            NotifSvc = notificationService;
        }


        /// <summary>
        /// Fait une notication d'un succès.
        /// </summary>
        /// <param name="message"></param>
        protected void NotificationSuccess(string titre, string message)
        {
            NotificationMessage messNotif = new NotificationMessage()
            {
                Summary = titre,
                Detail = message,
                Duration = 3000,
                Severity = NotificationSeverity.Success
            };

            NotifSvc.Notify(messNotif);
        }

        /// <summary>
        /// Fait une notication d'une erreur.
        /// </summary>
        /// <param name="message"></param>
        protected void NotificationError(string message)
        {
            NotificationMessage messNotif = new NotificationMessage()
            {
                Summary = "ERREUR",
                Detail = message,
                Duration = 3000,
                Severity = NotificationSeverity.Error
            };

            NotifSvc.Notify(messNotif);
        }

    }
}
