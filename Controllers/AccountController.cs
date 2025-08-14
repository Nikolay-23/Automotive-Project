using Automotive_Project.Data;
using Automotive_Project.Extensions;
using Automotive_Project.Migrations;
using Automotive_Project.Models;
using Automotive_Project.Services;
using Automotive_Project.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Automotive_Project.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly EmailSender _emailSender;
        public AccountController(ApplicationDbContext dbContext, EmailSender emailSender)
        {
            _dbContext = dbContext;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View(_dbContext.UserAccounts.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var hashed = PasswordHasher.HashPassword(model.Password.Trim());
            Console.WriteLine($"HASH: {hashed} (Length: {hashed.Length})");

            bool emailExists = await _dbContext.UserAccounts
                .AnyAsync(u => u.Email == model.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }


            var account = new UserAccount
            {
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = model.Email.Trim().ToLower(),
                Password = PasswordHasher.HashPassword(model.Password.Trim()) 
            };

            try
            {

                await _dbContext.UserAccounts.AddAsync(account);
                await _dbContext.SaveChangesAsync();

                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something went wrong while creating your account.");
                return View(model);
            }
        }

        public IActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {

                string hashedPassword = PasswordHasher.HashPassword(loginViewModel.Password.Trim());

                var user = await _dbContext.UserAccounts
                    .FirstOrDefaultAsync(x => x.Email == loginViewModel.Email && x.Password == hashedPassword);

                if (user != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("Name", user.FirstName),
                new Claim(ClaimTypes.Role, "User")
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity)
                    );

                    return RedirectToAction("SecurePage");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is not correct");
                }
            }

            return View(loginViewModel);
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Email))
            {
                return View(model);
            }

            string normalizedEmail = model.Email.Trim().ToLower();

            var user = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user == null)
            {
                
                ModelState.AddModelError("Email", "No account found with this email address.");
                return View(model);

            }

            if(user != null)
            {
                ModelState.AddModelError("Email", "Email is send");
                return View(model);
            }

                string token = GeneratePasswordResetToken();
            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddMinutes(15); 

            await _dbContext.SaveChangesAsync();

            string resetLink = Url.Action("ResetPassword", "Account",
                               new { token = token, email = user.Email }, Request.Scheme);

            _emailSender.SendEmail(
                user.Email,
                "Password Reset",
                $"<p>Click the link to reset your password:</p><p><a href='{resetLink}'>Reset Password</a></p>",
                $"Click the link to reset your password: {resetLink}"
            );

            return View("ForgotPassword");
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email
            };
            ViewBag.Token = token; 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string token)
        {

            if (!ModelState.IsValid)
                return View(model);

            var user = await _dbContext.UserAccounts
                .FirstOrDefaultAsync(u => u.Email == model.Email.Trim().ToLower() &&
                                          u.ResetPasswordToken == token &&
                                          u.ResetPasswordTokenExpiry > DateTime.UtcNow);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid or expired password reset token.");
                return View(model);
            }


            user.Password = PasswordHasher.HashPassword(model.Password.Trim()); 
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;

            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Password reset successfully. You can now log in.";
            return RedirectToAction("Login");
        }

        private string GeneratePasswordResetToken()
        {
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(32); 
            return Convert.ToBase64String(tokenBytes);
        }
    }
}
