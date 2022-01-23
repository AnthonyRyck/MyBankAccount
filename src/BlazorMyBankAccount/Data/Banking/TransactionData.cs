namespace BlazorMyBankAccount.Data.Banking
{
    public class TransactionData : BankingData, ITransactionData
    {

        public TransactionData(bankingContext bankingContext)
            : base(bankingContext)
        {
        }

        public async Task<Transactionobligatoire> AddTransaction(string nomTransaction, decimal montant, int jour, int idCompte, Typestransaction typetransac)
        {
            Transactionobligatoire transactionobligatoire = new Transactionobligatoire()
            {
                Idcompte = idCompte,
                Nomtransaction = nomTransaction,
                Jour = jour,
                Montant = montant,
                Type = typetransac
            };

            await context.AddAsync(transactionobligatoire);
            await context.SaveChangesAsync();
            return transactionobligatoire;
        }

        /// <inheritdoc cref="ITransactionData.GetTransactions"/>
        public async Task<List<Transactionobligatoire>> GetTransactions()
        {
            return await context.Transactionobligatoires
                            .Include(cpt => cpt.IdcompteNavigation)
                            .Include(typ => typ.Type)
                            .ToListAsync();
        }

        /// <inheritdoc cref="ITransactionData.GetAllComptes"/>
        public async Task<IEnumerable<Compte>> GetAllComptes()
        {
            return await context.Comptes.ToListAsync();
        }

        /// <inheritdoc cref="ITransactionData.GetAllTypes"/>
        public async Task<IEnumerable<Typestransaction>> GetAllTypes()
        {
            return await context.Typestransactions.ToListAsync();
        }
    }
}
