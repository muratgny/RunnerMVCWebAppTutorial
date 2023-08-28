using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club>? Clubs { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
