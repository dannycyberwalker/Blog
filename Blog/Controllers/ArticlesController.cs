using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using Blog.Models;
using Microsoft.Extensions.Configuration;
using Blog.Models.ViewModels;

namespace Blog.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext context;
        private IConfiguration Configuration;
        public int PageSize = 4;
        public ArticlesController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            Configuration = configuration;
        }
        [HttpGet]
        public IActionResult List(int PageId = 1)
        {
            if (Configuration["User"] == null)
                return RedirectToAction("Authorization", "Home");
            List<ListViewModel> models = new List<ListViewModel>();
            foreach(var article in context.Articles)
            {
                models.Add(new ListViewModel
                {
                    Article = article,
                    CountComments = context.Comments.Where(c => c.ArticleId == article.Id).Count(),
                    AuthorName = context.Users.FirstOrDefault(n => n.Id == article.UserId).NickName,
                    AuthorId = article.UserId,
                    UserId = JsonSerializer.Deserialize<User>(Configuration["User"]).Id
                });
            }
            return View(new ArticleListViewModel 
            {
                ListViewModels = models.OrderBy(a => a.Article.Id)
                .Skip((PageId - 1) * PageSize)
                .Take(PageSize).ToList(), 
                PagingInfo = new PagingInfo 
                {
                    CurrentPage = PageId, 
                    ItemsPerPage = PageSize, 
                    TotalItems = models.Count()
                }
            });
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
            if (ModelState.IsValid)
            {
                if (StringTags != null)
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
            }
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

        [HttpGet]
        public IActionResult Edit(int ArticleId) =>
           View(context.Articles.Where(a => a.Id == ArticleId).FirstOrDefault());

        [HttpPost]
        public IActionResult Edit(Article ArticleEdited)
        {
           Article ArticleFromDb = context.Articles
                .Where(a => a.Id == ArticleEdited.Id)
                .FirstOrDefault();
            ArticleFromDb.Headline = ArticleEdited.Headline;
            ArticleFromDb.PictureLink = ArticleEdited.PictureLink;
            ArticleFromDb.ShortDescription = ArticleEdited.ShortDescription;
            ArticleFromDb.Text = ArticleEdited.Text;
            context.SaveChanges();
            return RedirectToAction("Show", "Articles", new { ArticleId = ArticleEdited.Id }); ;
        }
    }
}
