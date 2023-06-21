using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Data;
using RunnerMVCWebAppTutorial.Interfaces;
using RunnerMVCWebAppTutorial.Models;
using RunnerMVCWebAppTutorial.Repositories;
using RunnerMVCWebAppTutorial.Services;
using RunnerMVCWebAppTutorial.ViewModels;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class RaceController : Controller
    {
        IRaceRepository _raceRepository;
        IPhotoService _photoService;
        public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
        {
            _raceRepository = raceRepository;
            _photoService = photoService;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRace(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.File);

                raceVM.Race.Image = result.Url.ToString();

                _raceRepository.Add(raceVM.Race);

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
    }
}
