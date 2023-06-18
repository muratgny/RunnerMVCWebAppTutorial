using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Data;
using RunnerMVCWebAppTutorial.Interfaces;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class RaceController : Controller
    {
        IRaceRepository _raceRepository;
        public RaceController(IRaceRepository raceRepository)
        {
            _raceRepository = raceRepository;
        }
        public async Task<IActionResult> Index()
        {
            var races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> DetailRace(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            return View(race);
        }
    }
}
