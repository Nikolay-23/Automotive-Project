using Microsoft.AspNetCore.Mvc;

namespace Automotive_Project.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
