using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Blog.Models;
using Microsoft.Extensions.Configuration;
using Blog.Models.ViewModels;
using System.Runtime.InteropServices;

namespace Blog.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext context;
        private IConfiguration Configuration;
        public ArticlesController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }
        [HttpGet]
        public IActionResult List()
        {
            if (Configuration["User"] == null)
                return RedirectToAction("Authorization", "Home");
            List<ListViewModel> models = new List<ListViewModel>();
            foreach(var i in context.Articles)
            {
                models.Add(new ListViewModel
                {
                    Article = i,
                    CountComments = context.Comments.Where(c => c.ArticleId == i.Id).Count()
                });
            }
            return View(models);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            if (Configuration["User"] == null)
                return RedirectToAction("Authorization", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Article article, string StringTags)
        { 
            if(StringTags != null)
            {
                article.Tags = new List<Tag>();
                foreach (string TagName in StringTags.Split(' '))
                {
                    article.Tags.Add(new Tag { Name = TagName });
                }
            }
            article.UserId = JsonSerializer.Deserialize<User>(Configuration["User"]).Id;
            context.Articles.Add(article);
            context.SaveChanges();
            return View();
        }


        [HttpGet]
        public IActionResult Show(int ArticleId)
        {
            return View(new ArticleViewModel 
            {
                Article = context.Articles.FirstOrDefault(a => a.Id == ArticleId),
                Comments = context.Comments.Where(c => c.ArticleId == ArticleId).ToList(),
                Tags = context.Tags.Where(t => t.ArticleId == ArticleId).ToList()
            });
        }
        [HttpPost]
        public IActionResult Show(string Author, string CommentText, int ArticleId)
        {
            context.Comments.Add(new Comment
            {
                Text = CommentText,
                Author = Author,
                ArticleId = ArticleId
            });
            context.SaveChanges();
            return View(new ArticleViewModel
            {
                Article = context.Articles.FirstOrDefault(a => a.Id == ArticleId),
                Comments = context.Comments.Where(c => c.ArticleId == ArticleId).ToList(),
                Tags = context.Tags.Where(t => t.ArticleId == ArticleId).ToList()
            });
        }
    }
}
