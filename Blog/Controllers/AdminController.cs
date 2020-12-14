using Blog.Models;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Blog.Services;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    /// <summary>
    /// Controller for admin panel
    /// </summary>
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        readonly private ApplicationDbContext context;
        readonly private ChartViewModelGenerator dataGenerator;
        public AdminController(ApplicationDbContext context, ChartViewModelGenerator dataGenerator)
        {
            this.context = context;
            this.dataGenerator = dataGenerator;
        }

        public IActionResult Index()
        {
            List<ChartViewModel<string>> chartsViewModel = new List<ChartViewModel<string>>
            {
               dataGenerator.Generate(
                        context.Comments
                        .Where(c => c.CreateTime.AddDays(30) > DateTime.Now)
                        .Select(d => d.CreateTime)
                        .ToList(),
                        DateTime.Now.AddDays(-30), DateTime.Now, 3, "Comments"),
               dataGenerator.Generate(
                        context.Users
                        .Where(c => c.CreateAccountTime.AddDays(30) > DateTime.Now)
                        .Select(d => d.CreateAccountTime)
                        .ToList(),
                        DateTime.Now.AddDays(-30), DateTime.Now, 3, "Users"),
               dataGenerator.Generate(
                        context.Articles
                        .Where(c => c.CreateTime.AddDays(30) > DateTime.Now)
                        .Select(d => d.CreateTime)
                        .ToList(),
                        DateTime.Now.AddDays(-30), DateTime.Now, 3, "Articles")
            };
            return View(chartsViewModel);
        }
    }
}
