using System.ComponentModel.DataAnnotations;

namespace BlazorMyBankAccount.ValidationModels
{
    public class ConfigValidation
    {
        [Required(ErrorMessage = "Il faut choisir une année de traitement")]
        [Range(2020, 2050, ErrorMessage = "Une année entre 2020 et 2050")]
        public int Annee { get; set; }

        [Required(ErrorMessage = "Il faut choisir un mois de traitement")]
        [Range(1, 12, ErrorMessage = "Un mois de l'année, 1 à 12...")]
        public int Mois { get; set; }

        [Required(ErrorMessage = "Il faut sélectionner un compte par default")]
        public Compte CompteParDefault { get; set; }
    }
}
