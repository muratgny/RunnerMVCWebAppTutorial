using Microsoft.AspNetCore.Mvc;
using RunnerMVCWebAppTutorial.Data;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class ClubController : Controller
    {
        AppDbContext _context;
        public ClubController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()//this "index" name must be also view's name under views/club folder. So it is seen directly
        {
            var clubs = _context.Clubs.ToList();
            return View(clubs);
        }
    }
}
