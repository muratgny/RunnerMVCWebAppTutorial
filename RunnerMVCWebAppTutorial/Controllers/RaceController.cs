using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Data;
using RunnerMVCWebAppTutorial.Interfaces;
using RunnerMVCWebAppTutorial.Models;
using RunnerMVCWebAppTutorial.Repositories;
using RunnerMVCWebAppTutorial.Services;
using RunnerMVCWebAppTutorial.ViewModels;
using System.Security.Claims;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class RaceController : Controller
    {
        IRaceRepository _raceRepository;
        IPhotoService _photoService;
        IHttpContextAccessor _httpContextAccessor;
        public RaceController(IRaceRepository raceRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<IActionResult> CreateRace()
        {
            var curUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var raceVM = new CreateRaceViewModel();
            raceVM.AppUserId = curUser;//burada aldığımız user id yi view e gönderiyoruz kş, create işlemini post ederken hangi user ın create ettiğini bilelim diye
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRace(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.File);

                var race = new Race()
                {
                    Id = raceVM.Id,
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Address = new Address()
                    {
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                        Street = raceVM.Address.Street,
                    },
                    RaceCategory = raceVM.RaceCategory,
                    Image = result.Url.ToString(),
                    AppUserId = raceVM.AppUserId,
                };

                _raceRepository.Add(race);

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }

        [HttpGet]
        public async Task<IActionResult> EditRace(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");

            var raceVM = new EditRaceViewModel();
            raceVM.Race = race;
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditRace(EditRaceViewModel raceVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceVM);
            }

            if (raceVM.File != null)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.File);

                if (result.Error != null)
                {
                    ModelState.AddModelError("Image", "Photo upload failed");
                    return View(raceVM);
                }

                raceVM.Race.Image = result.Url.ToString();
            }


            _raceRepository.Update(raceVM.Race);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null) return View("Error");
            return View(race);
        }

        [HttpPost, ActionName("DeleteRace")]
        public async Task<IActionResult> DeleteRaceForm(int id)
        {
            var raceDetails = await _raceRepository.GetByIdAsync(id);

            if (raceDetails == null)
            {
                return View("Error");
            }

            if (!string.IsNullOrEmpty(raceDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(raceDetails.Image);
            }

            _raceRepository.Delete(raceDetails);
            return RedirectToAction("Index");
        }
    }
}
