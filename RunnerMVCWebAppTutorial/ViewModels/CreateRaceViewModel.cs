using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.ViewModels
{
    public class CreateRaceViewModel
    {
        public Race Race { get; set; }
        public IFormFile File { get; set; }
    }
}
