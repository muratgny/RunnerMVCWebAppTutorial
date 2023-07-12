using Microsoft.AspNetCore.Mvc;
using RunnerMVCWebAppTutorial.Interfaces;
using RunnerMVCWebAppTutorial.ViewModels;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRespository.GetAllUserRaces();
            var userClubs = await _dashboardRespository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                UserRaces = userRaces,
                UserClubs = userClubs
            };
            return View(dashboardViewModel);
        }
    }
}
