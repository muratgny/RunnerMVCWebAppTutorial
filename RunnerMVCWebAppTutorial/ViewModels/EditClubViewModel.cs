using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.ViewModels
{
    public class EditClubViewModel
    {
        public Club Club { get; set; }
        public IFormFile? File { get; set; }
    }
}
