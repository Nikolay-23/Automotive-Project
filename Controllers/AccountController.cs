using Automotive_Project.Data;
using Automotive_Project.Models;
using Automotive_Project.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Automotive_Project.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDbContext _dbContext;

        public AccountController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
        public async Task<IActionResult> Registration(RegistrationViewModel registrationViewModel)
        {

            if (ModelState.IsValid)
            {
                UserAccount account = new UserAccount();
                
                 account.FirstName = registrationViewModel.FirstName;
                 account.LastName = registrationViewModel.LastName;
                 account.Email = registrationViewModel.Email;
                 account.Password = registrationViewModel.Password;

                try
                {
                    await _dbContext.UserAccounts.AddAsync(account);
                    await _dbContext.SaveChangesAsync();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registered successfully ";

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Please enter unique Email or Password");
                    return View(registrationViewModel);
                }
                return View();
                
            }
            return View(registrationViewModel);
        }


        public IActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {

                var user = await _dbContext.UserAccounts
                    .Where(x => x.Email == loginViewModel.Email && x.Password == loginViewModel.Password)
                    .FirstOrDefaultAsync();

                if(user != null)
                {
                  
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.FirstName),
                        new Claim(ClaimTypes.Role, "User")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

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

        
    }
}
