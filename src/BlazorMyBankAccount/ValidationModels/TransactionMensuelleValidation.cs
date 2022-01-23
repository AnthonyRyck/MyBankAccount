using System.ComponentModel.DataAnnotations;

namespace BlazorMyBankAccount.ValidationModels
{
    public class TransactionMensuelleValidation
    {
        [Required(ErrorMessage = "Il faut un compte")]
        public Compte CompteSelected { get; set; }

        [Required(ErrorMessage = "Il faut indiquer un montant")]
        [Precision(6, 2)]
        public decimal Montant { get; set; }

        [Required(ErrorMessage = "Il faut un nom de transaction")]
        [StringLength(25, ErrorMessage = "Le nom est trop long, 25 caractères max")]
        public string NomTransaction { get; set; }

        [Required(ErrorMessage = "Il faut indiquer un jour")]
        [Range(1, 31, ErrorMessage = "Un jour de mois, entre 1 et 31")]
        public int Jour { get; set; } = 1;

        [Required(ErrorMessage = "Il faut indiquer quel type de transaction")]
        public Typestransaction Typestransaction { get; set; }
    }
}
