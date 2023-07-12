using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.ViewModels
{
    public class DashboardViewModel
    {
        public List<Club>? UserClubs { get; set; }
        public List<Race>? UserRaces { get; set; }
    }
}
