using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EF_Project.Models;
using EF_Project.Data;
using MedCore.ViewModels;
using System.Threading.Tasks;

namespace MedCore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MedCoreContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            MedCoreContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    NationalId = model.NationalId,
                    DateOfBirth = model.DateOfBirth
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);

                    if (model.Role == "Patient")
                    {
                        Patient patient = new Patient
                        {
                            ApplicationUserId = user.Id,
                            BloodType = model.BloodType ?? "Unknown",
                            Allergies = model.Allergies,
                            ChronicConditions = model.ChronicConditions
                        };
                        _context.Patients.Add(patient);
                    }
                    else if (model.Role == "Doctor")
                    {
                        Doctor doctor = new Doctor
                        {
                            ApplicationUserId = user.Id,
                            LicenseNumber = model.LicenseNumber ?? "N/A",
                            YearsOfExperience = model.YearsOfExperience ?? 0,
                            HourlyRate = model.HourlyRate,
                            SpecialityId = model.SpecialityId ?? 1
                        };
                        _context.Doctors.Add(doctor);
                    }

                    await _context.SaveChangesAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}