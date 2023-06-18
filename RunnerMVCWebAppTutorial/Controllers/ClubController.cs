using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Data;
using RunnerMVCWebAppTutorial.Interfaces;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class ClubController : Controller
    {
        private IClubRepository _clubRepository;
        public ClubController(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }
        public async Task<IActionResult> Index()//this "index" name must be also view's name under views/club folder. So it is seen directly
        {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> DetailClub(int id)//bunun üzerinde route verebilirdik ama program.cs dosyasında mapper olduğundan gerek kalmadı
        {
            var club = await _clubRepository.GetByIdAsync(id);
            //var club = _context.Clubs.Include(a => a.Address).FirstOrDefault(x => x.Id == id);//Burada başka bir tablodan 
            //adres bilgisini çektiğimiz için hata veriyor. şimdilik sorunu include ile adress tablosunu ekleyerek çözdük.
            //Ama bu hiç verimli bir yöntem değil. Repository de joint işlemleri ile çekip yapacağız bu işi aslında.
            return View(club);
        }
    }
}
