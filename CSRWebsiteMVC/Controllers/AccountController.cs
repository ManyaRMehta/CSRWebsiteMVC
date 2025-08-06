using CSRWebsiteMVC.Data;
using CSRWebsiteMVC.Models;
using CSRWebsiteMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSRWebsiteMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Console.WriteLine("Register POST called");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid");
                return View(model);
            }

            string? profilePath = null;

            // Handle profile image upload
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profilepics");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                profilePath = "/profilepics/" + uniqueFileName;
            }
            else
            {
                profilePath = "/profilepics/default.png";
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                PhotoPath = profilePath,
                EmployeeId = "EMP-" + Guid.NewGuid().ToString().Substring(0, 8),
                Department = "Not Set",
                Title = "Not Set"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            Console.WriteLine($"Create result: {result.Succeeded}");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Missions");

            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Register error: {error.Description}");
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }



        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            Console.WriteLine($"Login result: {result.Succeeded}");

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Missions");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            var applications = await _context.MissionApplications
                .Include(a => a.Mission)
                .Where(a => a.UserId == user.Id)
                .ToListAsync();

            var viewModel = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PhotoPath = user.PhotoPath,
                Applications = applications
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            var model = new EditProfileViewModel
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                ExistingPhotoPath = user.PhotoPath
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.NewProfileImage != null && model.NewProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profilepics");
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.NewProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.NewProfileImage.CopyToAsync(stream);
                }

                user.PhotoPath = "/profilepics/" + uniqueFileName;
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);
            TempData["ToastMessage"] = "Profile updated successfully!";

            return RedirectToAction("Profile");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                PhotoPath = "/profilepics/default.png", // default image
                EmployeeId = "ADMIN-" + Guid.NewGuid().ToString().Substring(0, 8),
                Department = "Admin Dept",
                Title = "Admin"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["Success"] = "New admin account created successfully.";
                return RedirectToAction("Index", "Missions");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }







    }
}
