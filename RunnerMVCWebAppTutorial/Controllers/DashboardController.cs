using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunnerMVCWebAppTutorial.Interfaces;
using RunnerMVCWebAppTutorial.ViewModels;
using System.Security.Claims;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<IActionResult> EditUserProfile()//"string id" we can pass this to this method but it is not a good idea to pass userid 
                                                          //here. Because it can be seen in the front part and iti is not good for security. We will use IHttpContextAccessor
        {
            var curUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _dashboardRespository.GetUserById(curUser); //we have to get user info to be able to easily and securely get info

            if(user == null) return View("Error");

            var editUserProfileVM = new EditUserProfileViewModel()
            {
                Id = curUser,
                City = user.City,
                State = user.State,
                Mileage = user.Mileage,
                Pace = user.Pace,
                ProfileImageUrl = user.ProfileImageUrl
            };
            return View(editUserProfileVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);
            }

            var user = await _dashboardRespository.GetByIdNoTracking(editVM.Id);

            if (user == null)
            {
                return View("Error");
            }

            if (editVM.Image != null) // only update profile image
            {
                var photoResult = await _photoService.AddPhotoAsync(editVM.Image);

                if (photoResult.Error != null)
                {
                    ModelState.AddModelError("Image", "Failed to upload image");
                    return View("EditUserProfile", editVM);
                }

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    _ = _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }

                user.ProfileImageUrl = photoResult.Url.ToString();
                editVM.ProfileImageUrl = user.ProfileImageUrl;

                _dashboardRespository.Update(user);

                return View(editVM);
            }

            user.City = editVM.City;
            user.State = editVM.State;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;

            _dashboardRespository.Update(user);

            return RedirectToAction("Index");
        }
    }
}
