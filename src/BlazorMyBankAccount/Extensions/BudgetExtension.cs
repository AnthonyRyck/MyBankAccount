namespace BlazorMyBankAccount.Extensions
{
    public static class BudgetExtension
    {


        public static BudgetCompteView ToBudgetCompteView(this Budget source)
        {
            return new BudgetCompteView()
            {
                Budget = source,
                Compte = source.IdcompteNavigation
            };
        }
    }
}
