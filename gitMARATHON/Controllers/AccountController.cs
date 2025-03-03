using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using gitMARATHON.Models;
using Microsoft.AspNetCore.Authorization;

namespace gitMARATHON.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string AdminSecretPhrase = "FitEduDepGitMarathon";

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Перевірка, чи вже існує користувач з таким email
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "User with this email already exists.");
                return View(model);
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                DisplayName = model.DisplayName,
                IsAdmin = false
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            // Генерація токена для підтвердження email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail", "Account",
                new { userId = user.Id, token = token },
                protocol: HttpContext.Request.Scheme);

            var emailSender = new EmailSender();
            await emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by clicking this link: {callbackUrl}");

            return RedirectToAction("CheckEmail", "Account");
        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return BadRequest("Invalid token or user ID.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return RedirectToAction("Login");

            return BadRequest("Email confirmation failed.");
        }


        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "You need to confirm your email before logging in.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index() => View();

        public IActionResult CheckEmail()
        {
            return View();
        }

    }
}
