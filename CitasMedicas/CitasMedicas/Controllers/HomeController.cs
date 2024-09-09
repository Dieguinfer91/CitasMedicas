﻿using CitasMedicas.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CitasMedicas.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //HttpContext.Session.SetString("UserStatus", "LoggedIn");
            
            return View();
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            //HttpContext.Session.SetString("UserStatus", "LoggedIn");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
