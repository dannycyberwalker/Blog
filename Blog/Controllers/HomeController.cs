using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Models;
using Microsoft.Extensions.Configuration;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context;
        private IConfiguration Configuration;
        public HomeController(ApplicationDbContext context, IConfiguration conf)
        {
            this.context = context;
            Configuration = conf;
        }
        [HttpGet]
        public IActionResult Authorization()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Authorization(string Email, string Password)
        {
            User CurrentUser = context.Users.FirstOrDefault(u => u.Email == Email);
            if (CurrentUser != null && CurrentUser.Password == Password)
                 Configuration["User"] = JsonSerializer.Serialize(CurrentUser);  
            else
                return RedirectToAction("AuthorizationError", "Home");
            return RedirectToAction("List", "Articles");
        }
        public IActionResult AuthorizationError()
        {
            return View();
        }
    }
}
