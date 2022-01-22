using System.ComponentModel.DataAnnotations;

namespace BlazorMyBankAccount.ValidationModels
{
    public class CompteValidation
    {
        [Required(ErrorMessage = "Il faut un nom de compte")]
        [StringLength(25, ErrorMessage = "Le nom est trop long, 25 caractères max")]
        public string NomCompte { get; set; }

        [StringLength(50, ErrorMessage = "Le nom est trop long, 25 caractères max")]
        public string Description { get; set; }
    }
}
