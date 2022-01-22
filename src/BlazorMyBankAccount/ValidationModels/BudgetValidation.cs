using System.ComponentModel.DataAnnotations;

namespace BlazorMyBankAccount.ValidationModels
{
    public class BudgetValidation
    {
        [Required(ErrorMessage = "Il faut un nom de budget")]
        [StringLength(25, ErrorMessage = "Le nom est trop long, 25 caractères max")]
        public string NomBudget { get; set; }

        [StringLength(50, ErrorMessage = "Le nom est trop long, 50 caractères max")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Il faut sélectionner un compte pour le stockage")]
        public Compte CompteId { get; set; }
    }
}
