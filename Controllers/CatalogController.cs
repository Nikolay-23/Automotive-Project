﻿using Microsoft.AspNetCore.Mvc;

namespace Automotive_Project.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
