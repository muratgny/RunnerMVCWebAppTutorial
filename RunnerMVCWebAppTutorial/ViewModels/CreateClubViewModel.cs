using RunnerMVCWebAppTutorial.Data.Enum;
using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.ViewModels
{
    public class CreateClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public string AppUserId { get; set; }
        public IFormFile File { get; set; }
    }
}
