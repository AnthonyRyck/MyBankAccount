namespace BlazorMyBankAccount.Data.Banking
{
    public abstract class BankingData
    {
        protected readonly bankingContext context;

        public BankingData(bankingContext dbContext)
        {
            context = dbContext;
        }
    }
}
