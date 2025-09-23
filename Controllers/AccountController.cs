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
        private readonly CustomUserManager _userManager;
        private readonly CustomRoleManager _roleManager;
        private readonly CustomSignInManager _signInManager;
        private readonly EmailSender _emailSender;

        public AccountController(
            CustomUserManager userManager,
            CustomRoleManager roleManager,
            CustomSignInManager signInManager,
            EmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public IActionResult Registration() => View();

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                return View(model);
            }

            var user = new UserAccount
            {
                UserName = model.UserName.Trim(),
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = model.Email.Trim().ToLower()
            };

            await _userManager.CreateAsync(user, model.Password);

            // Ensure default role exists
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateRoleAsync("User");
            }

            await _userManager.AddToRoleAsync(user, "User");

            TempData["SuccessMessage"] = "Registration successful! You can now log in.";
            return RedirectToAction("Login");
        }
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var success = await _signInManager.PasswordSignInAsync(model.Email, model.Password);
            if (!success)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

        public IActionResult ForgotPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(model.Email))
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email.Trim().ToLower());
            if (user == null)
            {
                ModelState.AddModelError("Email", "No account found with this email address.");
                return View(model);
            }

            // Generate reset token
            string token = GeneratePasswordResetToken();
            user.ResetPasswordToken = token;
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);

            string resetLink = Url.Action(
                "ResetPassword",
                "Account",
                new { token = token, email = user.Email },
                Request.Scheme
            );

            _emailSender.SendEmail(
                user.Email,
                "Password Reset",
                $"<p>Click the link to reset your password:</p><p><a href='{resetLink}'>Reset Password</a></p>",
                $"Click the link to reset your password: {resetLink}"
            );

            TempData["SuccessMessage"] = "Password reset email sent!";
            return View("ForgotPassword");
        }

        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Email = email };
            ViewBag.Token = token;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string token)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email.Trim().ToLower());
            if (user == null || user.ResetPasswordToken != token || user.ResetPasswordTokenExpiry < DateTime.UtcNow)
            {
                ModelState.AddModelError("", "Invalid or expired password reset token.");
                return View(model);
            }

            user.Password = PasswordHasher.HashPassword(model.Password.Trim());
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiry = null;

            await _userManager.UpdateAsync(user);

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
