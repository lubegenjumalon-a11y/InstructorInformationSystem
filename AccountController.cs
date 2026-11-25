using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using InstructorInfoSystem.Data;
using InstructorInfoSystem.Models;
using InstructorInfoSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstructorInfoSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<UserAccount> _passwordHasher;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<UserAccount>();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = model.Email.Trim();

            bool emailExists = await _context.UserAccounts
                .AnyAsync(account =>
                    account.Email.ToLower() == email.ToLower());

            if (emailExists)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "An account with this email already exists.");

                return View(model);
            }

            var account = new UserAccount
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = email
            };

            account.PasswordHash = _passwordHasher.HashPassword(
                account, model.Password);

            _context.UserAccounts.Add(account);
            await _context.SaveChangesAsync();

            TempData["Message"] =
                $"Registration successful. Your account ID is {account.UserAccountId}. You may now log in.";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var value = model.EmailOrId.Trim();

            UserAccount? account;

            if (int.TryParse(value, out var accountId))
            {
                account = await _context.UserAccounts
                    .FirstOrDefaultAsync(a => a.UserAccountId == accountId);
            }
            else
            {
                var normalizedEmail = value.ToLowerInvariant();

                account = await _context.UserAccounts
                    .FirstOrDefaultAsync(a =>
                        a.Email.ToLower() == normalizedEmail);
            }

            if (account != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(
                    account,
                    account.PasswordHash,
                    model.Password);

                if (result != PasswordVerificationResult.Failed)
                {
                    var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    account.UserAccountId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    account.Email),

                new Claim(
                    "FirstName",
                    account.FirstName),

                new Claim(
                    "LastName",
                    account.LastName)
            };

                    var identity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal);

                    TempData["Message"] =
                        $"Welcome back, {account.FirstName}!";

                    return RedirectToAction("Index", "Home");
                }
            }

            const string loginError =
                "Login failed. The email address, account ID, or password is incorrect.";

            ModelState.AddModelError(string.Empty, loginError);
            TempData["Message"] = loginError;

            return View(model);
        }


        [HttpGet]
        public IActionResult Profile()
        {
            if (User.Identity?.IsAuthenticated != true)
            {
                return Challenge();
            }

            ViewBag.AccountId = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            ViewBag.FirstName = User.FindFirst("FirstName")?.Value;
            ViewBag.LastName = User.FindFirst("LastName")?.Value;
            ViewBag.Email = User.FindFirst(ClaimTypes.Name)?.Value;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["Message"] = "You have successfully logged out.";

            return RedirectToAction(nameof(Login));
        }

    }
}
