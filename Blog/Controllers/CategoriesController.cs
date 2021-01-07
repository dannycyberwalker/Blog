using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blog.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext context;
        public CategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index() => View(context.Categories);

        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public IActionResult Create(Category category)
        {
            context.Categories.Add(category);
            context.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }
        public IActionResult Edit(int id) 
        {
            Category category = context.Categories.Single(c => c.Id == id);
            return View(category); 
        }
        [HttpPost]
        public IActionResult Edit(Category editedCategory)
        {
            Category fromDb = context.Categories.Single(c => c.Id == editedCategory.Id);
            fromDb.Name = editedCategory.Name;
            context.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Category category = context.Categories.Single(c => c.Id == id);
            context.Categories.Remove(category);
            context.SaveChanges();
            return RedirectToAction("Index", "Categories");
        }
    }
}
