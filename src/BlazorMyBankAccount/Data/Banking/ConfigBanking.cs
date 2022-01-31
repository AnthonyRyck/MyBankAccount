namespace BlazorMyBankAccount.Data.Banking
{
    public class ConfigBanking : BankingData, IConfigBanking
    {
        public ConfigBanking(bankingContext dbContext) 
            : base(dbContext)
        {
        }

        /// <inheritdoc cref="IConfigBanking.GetConfiguration"/>
        public async Task<Configbank> GetConfiguration()
        {
            return await context.Configbanks.Include(cpt => cpt.IdcomptedefaultNavigation)
                                            .FirstOrDefaultAsync();
        }

        /// <inheritdoc cref="IConfigBanking.GetComptes"/>
        public async Task<IEnumerable<Compte>> GetComptes()
        {
            return await context.Comptes.ToListAsync();
        }

        /// <inheritdoc cref="IConfigBanking.AddOrUpdate(Compte, int, int)"/>
        public async Task<Configbank> AddOrUpdate(Compte compteParDefault, int annee, int mois)
        {
            Configbank configbank = new Configbank()
            {
                Annee = annee,
                Mois = mois,
                Idcomptedefault = compteParDefault.Idcompte
            };

            var actualConfig = await GetConfiguration();
            if(actualConfig != null)
            {
                context.Configbanks.Remove(actualConfig);
                await context.SaveChangesAsync();
            }
                        
            await context.AddAsync(configbank);
            await context.SaveChangesAsync();

            return configbank;
        }


        public async Task CreateNewMonth(int idCompteDefaut, int annee, int mois)
        {
            // Récupération des transactions mensuelles.
            var allTransac = await context.Transactionobligatoires.ToListAsync();

            // Créer dans le suivi de compte, les lignes "obligatoires".
            foreach (var item in allTransac)
            {
                Suivicompte nouvelleLigne = new Suivicompte()
                {
                    Idcompte = idCompteDefaut,
                    Idannee = annee,
                    Idmois = mois,
                    Type = item.Type,
                    Montant = item.Montant,
                    Nomtransaction = item.Nomtransaction,
                    Isvalidate = false,
                    Commentaire = item.Commentaire
                };

                await context.AddAsync(nouvelleLigne);
            }

            await context.SaveChangesAsync();
        }

    }
}
