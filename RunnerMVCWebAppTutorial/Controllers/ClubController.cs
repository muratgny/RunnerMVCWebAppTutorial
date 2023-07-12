using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Data;
using RunnerMVCWebAppTutorial.Interfaces;
using RunnerMVCWebAppTutorial.Models;
using RunnerMVCWebAppTutorial.ViewModels;
using System.Security.Claims;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class ClubController : Controller
    {
        private IClubRepository _clubRepository;
        private IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
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

        
        public async Task<IActionResult> CreateClub()//Sayfayı ilk yüklemede göstermek için kullanılan metod
        {
            var curUser = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var clubVM = new CreateClubViewModel();
            clubVM.AppUserId = curUser;//burada aldığımız user id yi view e gönderiyoruz kş, create işlemini post ederken hangi user ın create ettiğini bilelim diye
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub(CreateClubViewModel clubVM)//sayfanın data post ettiği zamanki halini kontrol eden metod
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.File);

                var club = new Club()
                {
                    Id = clubVM.Id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Address = new Address()
                    {
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                        Street = clubVM.Address.Street,
                    },
                    ClubCategory = clubVM.ClubCategory,
                    AppUserId = clubVM.AppUserId,
                    Image = result.Url.ToString(),

            }; 

                _clubRepository.Add(club);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }

        [HttpGet]
        public async Task<IActionResult> EditClub(int id) 
        {
            var club = await _clubRepository.GetByIdAsync(id);

            if(club == null) return View("Error");

            var clubVM = new EditClubViewModel();
            clubVM.Club = club;

            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditClub(EditClubViewModel clubVM)//sayfanın data post ettiği zamanki halini kontrol eden metod
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            if (clubVM.File != null)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.File);

                if (result.Error != null)
                {
                    ModelState.AddModelError("Image", "Photo upload failed");
                    return View(clubVM);
                }

                clubVM.Club.Image = result.Url.ToString();
            }


            _clubRepository.Update(clubVM.Club);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> DeleteClub(int id)//Burası delete page i açmak için
        {
            var club = await _clubRepository.GetByIdAsync(id);

            if (club == null) return View("Error");


            return View(club);
        }

        [HttpPost, ActionName("DeleteClub")]//buradaki deleteClub view deki submit formdan emir almak için yazdığımız metod
        public async Task<IActionResult> DeleteClubForm(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);

            if (club == null) return View("Error");


            if (!string.IsNullOrEmpty(club.Image))
            {
                _ = _photoService.DeletePhotoAsync(club.Image);
            }

            _clubRepository.Delete(club);
            return RedirectToAction("Index");

        }
    }
}
