using System.ComponentModel.DataAnnotations;

namespace BlazorMyBankAccount.ValidationModels
{
    public class TransactionValidation
    {
        [Required(ErrorMessage = "Il faut un nom de transaction")]
        [StringLength(25, ErrorMessage = "Le nom est trop long, 25 caractères max")]
        public string NomTransaction { get; set; }

        [Required(ErrorMessage = "Il faut indiquer un montant")]
        [Precision(6, 2)]
        public decimal Montant { get; set; }


        public DateTime? DateTransaction { get; set; }

        [Required(ErrorMessage = "Il faut indiquer un type de transaction")]
        public Typestransaction TypeTransac { get; set; }


        public bool IsValide { get; set; } = false;
    }
}
