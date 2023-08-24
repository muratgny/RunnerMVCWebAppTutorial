namespace RunnerMVCWebAppTutorial.ViewModels
{
    public class EditUserProfileViewModel
    {
        public string Id { get; set; }
        public int? Pace { get; set; }//If we dont put these question marks here, the modelstate produce problem when the user
                                      //submit form with null value to these properties 
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public IFormFile? Image { get; set; }
    }
}
