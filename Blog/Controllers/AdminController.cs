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
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        readonly private ApplicationDbContext context;
        readonly private IDataGenerator dataGenerator; 
        public AdminController(ApplicationDbContext context, IDataGenerator dataGenerator)
        {
            this.context = context;
            this.dataGenerator = dataGenerator;
        }
        
        public IActionResult Index()
        {
            List<ChartViewModel> chartsViewModel = new List<ChartViewModel> 
            { 
                new ChartViewModel
                {
                    LineName = "Comments",
                    XName = "Time",
                    YName = "Count",
                    JSONArray = dataGenerator.Generate(
                        context.Comments
                        .Where(c => c.CreateTime.AddDays(30) > DateTime.Now)
                        .Select(d => d.CreateTime)
                        .ToList(), 
                        DateTime.Now.AddDays(-30), DateTime.Now, 1)
                },
                new ChartViewModel
                {
                    LineName = "Users",
                    XName = "Time",
                    YName = "Count",
                    JSONArray = dataGenerator.Generate(
                        context.Users
                        .Where(c => c.CreateAccountTime.AddDays(30) > DateTime.Now)
                        .Select(d => d.CreateAccountTime)
                        .ToList(),
                        DateTime.Now.AddDays(-30), DateTime.Now, 1)
                },
                new ChartViewModel
                {
                    LineName = "Articles",
                    XName = "Time",
                    YName = "Count",
                    JSONArray = dataGenerator.Generate(
                        context.Articles
                        .Where(c => c.CreateTime.AddDays(30) > DateTime.Now)
                        .Select(d => d.CreateTime)
                        .ToList(),
                        DateTime.Now.AddDays(-30), DateTime.Now, 1)
                }
            };
            return View(chartsViewModel);
        }
    }
}
