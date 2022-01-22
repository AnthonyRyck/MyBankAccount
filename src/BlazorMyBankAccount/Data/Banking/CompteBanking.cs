namespace BlazorMyBankAccount.Data.Banking
{
    public class CompteBanking : BankingData, ICompteBanking
    {
        public CompteBanking(bankingContext dbContext)
            : base(dbContext)
        {
        }

        #region Implement ICompteBanking

        /// <inheritdoc cref="ICompteBanking.AddAccount(Compte)"/>
        public async Task AddAccount(Compte account)
        {
            await context.Comptes.AddAsync(account);
            await context.SaveChangesAsync();
        }

        /// <inheritdoc cref="ICompteBanking.AddAccount(string, string)"/>
        public async Task<Compte> AddAccount(string accountName, string description)
        {
            Compte compte = new Compte();
            compte.Nomcompte = accountName;
            compte.Description = description;

            await context.Comptes.AddAsync(compte);
            await context.SaveChangesAsync();

            return compte;
        }

        /// <inheritdoc cref="ICompteBanking.GetAccount"/>
        public Task<Compte> GetAccount()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ICompteBanking.GetAccounts"/>
        public async Task<List<Compte>> GetAccounts()
        {
            return await context.Comptes.ToListAsync();
        }

        #endregion
    }
}
