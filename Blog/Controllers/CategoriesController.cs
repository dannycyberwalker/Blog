using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public IActionResult Index() => View(_context.Categories);

        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }
        public IActionResult Edit(int id) 
        {
            Category category = _context.Categories.Single(c => c.Id == id);
            return View(category); 
        }
        [HttpPost]
        public IActionResult Edit(Category editedCategory)
        {
            Category fromDb = _context.Categories.Single(c => c.Id == editedCategory.Id);
            fromDb.Name = editedCategory.Name;
            _context.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Single(c => c.Id == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }
    }
}
