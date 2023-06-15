using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Data;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class RaceController : Controller
    {
        AppDbContext _context;
        public RaceController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var races = _context.Races.ToList();
            return View(races);
        }
    }
}
