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
        private readonly ApplicationDbContext _context;
        private readonly ChartViewModelGenerator _dataGenerator;
        public AdminController(ApplicationDbContext context, ChartViewModelGenerator dataGenerator)
        {
            _context = context;
            _dataGenerator = dataGenerator;
        }

        [HttpGet]
        public IActionResult Index() => View();
        
        /// <param name="from">Date in string representation</param>
        /// <param name="to">Date in string representation</param>
        /// <returns>View model for plotly chart</returns>
        [HttpGet]
        public JsonResult GetStatistics(string tableName,string from, string to , int daysInOneStep)
        {
            Dictionary<string, IQueryable<ICreateTime>> tables = new()
            {
                {"Comments", _context.Comments},
                {"Articles", _context.Articles},
                {"Users", _context.Users}
            };
            ChartViewModel<string> response;

            //Validating request value. 
            if (!(DateTime.TryParse(from, out var From) && DateTime.TryParse(to, out var To)))
            {
                From = DateTime.Now.AddDays(-30);
                To = DateTime.Now;
            }
            //366 because you cannot request statistics that are more than a year old.
            if (From.AddDays(366) < To)
            {
                From = DateTime.Now.AddDays(-30);
                To = DateTime.Now;
            }
            if (daysInOneStep < 1 || From.AddDays(daysInOneStep) >= To)
            {
                daysInOneStep = 1;
            }
            
            
            if (tables.ContainsKey(tableName))
            {
                IEnumerable<DateTime> dates = tables[tableName]
                    .Select(d => d.CreateTime)
                    .Where(c => c.AddDays(To.Subtract(From).Days) > To);
                
                response = _dataGenerator.Generate(dates, From, To, daysInOneStep, tableName);
            }
            else
            {
                response = new ChartViewModel<string>()
                {
                    Title = "anyString",
                    XValues = new List<string>(),
                    YValues = new List<int>()
                };
            }
            return Json(response);
        }
    }
}
