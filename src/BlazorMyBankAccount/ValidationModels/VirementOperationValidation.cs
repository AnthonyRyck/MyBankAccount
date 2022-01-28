using System.ComponentModel.DataAnnotations;

namespace BlazorMyBankAccount.ValidationModels
{
	public class VirementOperationValidation
	{
        [Required(ErrorMessage = "Il faut sélectionner un compte")]
        public Compte CompteArrive { get; set; }

        [Required(ErrorMessage = "Il faut un nom de transaction")]
        [StringLength(25, ErrorMessage = "Le nom est trop long, 25 caractères max")]
        public string NomTransaction { get; set; }

        [Required(ErrorMessage = "Il faut indiquer un montant")]
        [Precision(6, 2)]
        public decimal Montant { get; set; }

        [Required(ErrorMessage = "Il faut indiquer une date")]
        public DateTime DateTransaction { get; set; }

		public Budget? Budget { get; set; }
	}
}
