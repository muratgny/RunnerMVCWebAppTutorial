using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunnerMVCWebAppTutorial.Data;
using RunnerMVCWebAppTutorial.Models;
using RunnerMVCWebAppTutorial.ViewModels;

namespace RunnerMVCWebAppTutorial.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,AppDbContext context)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            var loginVM = new LoginViewModel();//burada boş bir loginVM oluşturup sayfaya göndermizin sebebi eğer kullanıcı
            //bilgileri girerken sayfayı falan yenilerse bilgiler kaybolmasın diye
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if (user != null)
            {
                //User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    //Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Race");
                    }
                }
                //Password is incorrect. Burada böyle hata mesajı verilmez normalde. profesyonel değil bu yöntem
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginVM);
            }
            //User not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginVM);
        }
    }
}
