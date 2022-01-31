using BankDataAccess;

namespace BlazorMyBankAccount.Data
{
    public class DataInitializer
    {
        internal static async Task InitIdentityData(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            var roles = Enum.GetNames(typeof(Role));

            foreach (var role in roles)
            {
                // User est juste pour l'affichage.
                if (role == Role.SansRole.ToString())
                    continue;

                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Création de l'utilisateur Root.
            var user = await userManager.FindByNameAsync("root");

            if (user == null)
            {
                var poweruser = new IdentityUser
                {
                    UserName = "root",
                    Email = "root@email.com",
                    EmailConfirmed = true
                };
                string userPwd = "Azerty123!";
                var createPowerUser = await userManager.CreateAsync(poweruser, userPwd);
                if (createPowerUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(poweruser, Role.Admin.ToString());
                }
            }
        }
        
        /// <summary>
        /// Ajout des donnees pour le context de "Bank"
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        internal async static Task InitDataBank(bankingContext dbContext)
        {
            await InitTransaction(dbContext);
            await InitDateTraitement(dbContext);
            await InitCompte(dbContext);
            await InitTypeBudget(dbContext);

            await dbContext.SaveChangesAsync();
        }

        private async static Task InitTransaction(bankingContext dbContext)
        {
            Typestransaction typePrevision = new Typestransaction();
            typePrevision.Nom = TransactionType.Prévision.ToString();
            typePrevision.Description = "Prévision";
            await dbContext.AddAsync(typePrevision);

            Typestransaction typeVirement = new Typestransaction();
            typeVirement.Nom = TransactionType.Virement.ToString();
            typeVirement.Description = "Virement bancaire";
            await dbContext.AddAsync(typeVirement);

            Typestransaction typeCarte = new Typestransaction();
            typeCarte.Nom = TransactionType.Carte.ToString();
            typeCarte.Description = "Paiement par carte";
            await dbContext.AddAsync(typeCarte);

            Typestransaction typePrelevement = new Typestransaction();
            typePrelevement.Nom = TransactionType.Prélèvement.ToString();
            typePrelevement.Description = "Prélèvement bancaire";
            await dbContext.AddAsync(typePrelevement);

            Typestransaction typeCheque = new Typestransaction();
            typeCheque.Nom = TransactionType.Chèque.ToString();
            typeCheque.Description = "Paiement par chèque";
            await dbContext.AddAsync(typeCheque);
        }

        private async static Task InitDateTraitement(bankingContext dbContext)
        {
            int anneeEnCours = DateTime.Now.Year;
            Anneetraitement anneetraitement = new Anneetraitement();
            anneetraitement.Annee = anneeEnCours;
            await dbContext.AddAsync(anneetraitement);

            for (int mois = 1; mois <= 12; mois++)
            {
                Moistraitement moistraitement = new Moistraitement();
                moistraitement.Mois = mois;
                await dbContext.AddAsync(moistraitement);
            }
        }

        private async static Task InitCompte(bankingContext dbContext)
        {
            Compte compteCourant = new Compte();
            compteCourant.Nomcompte = "Compte courant/dépot";
            compteCourant.Description = "Compte de dépot";
            await dbContext.AddAsync(compteCourant);
        }

        private async static Task InitTypeBudget(bankingContext dbContext)
        {
            Typebudget typeEpargne = new Typebudget();
            typeEpargne.Nomtypebudget = "Epargne";
            await dbContext.AddAsync(typeEpargne);

            Typebudget typePrevDepense = new Typebudget();
            typePrevDepense.Nomtypebudget = "Prévision dépense";
            await dbContext.AddAsync(typePrevDepense);
        }
    }
}
