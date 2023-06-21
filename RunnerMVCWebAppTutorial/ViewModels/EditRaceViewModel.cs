using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.ViewModels
{
    public class EditRaceViewModel
    {
        public Race Race { get; set; }
        public IFormFile? File { get; set; }
    }
}
