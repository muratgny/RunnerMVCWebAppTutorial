using Microsoft.AspNetCore.Mvc;
using RunnerMVCWebAppTutorial.Interfaces;
using RunnerMVCWebAppTutorial.ViewModels;
using System.Security.Claims;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class UserController : Controller
    {
        IUserRepository _userRepository;
        IHttpContextAccessor _httpContextAccessor;
        public UserController(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpGet("users")]//Here, we put "users" to put address line "users" not "user". Default it will put "user" word.
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsersAsync();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                    ProfileImageUrl = user.ProfileImageUrl
                };
                result.Add(userViewModel);
            }

            return View(result);
        }

        public async Task<IActionResult> Detail(string id)//it must be "id" nor "userId" or anything else
        {
            var user = await _userRepository.GetByIdAsync(id);

            var userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage
            };
            return View(userDetailViewModel);
        }

        
    }
}
