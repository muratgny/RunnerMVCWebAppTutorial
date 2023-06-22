using System.ComponentModel.DataAnnotations;

namespace RunnerMVCWebAppTutorial.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; } //Burada validation için kurallar koyduk fakat normelde burada modelin içinde yapılmaz
        //dışarıda baska bir validation service ile yapılır.
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
