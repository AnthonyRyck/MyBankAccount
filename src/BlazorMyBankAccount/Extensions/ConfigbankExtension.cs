namespace BlazorMyBankAccount.Extensions
{
    public static class ConfigbankExtension
    {
        public static string GetMois(this Configbank source)
        {
            if(source == null)
                return "Aucun mois sélectionné";

            return new DateOnly(source.Annee, source.Mois, 1).ToString("MMMM") + " " + source.Annee;
        }
    }
}
