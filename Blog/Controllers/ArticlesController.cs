using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Blog.Models;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog.Controllers
{
    public class ArticlesController : Controller
    {
        readonly private int PageSize = 4;
        readonly private ApplicationDbContext context;
        readonly private UserManager<User> userManager;
        public ArticlesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string name, int page = 1,
            SortState sortOrder = SortState.DateDesc)
        {
            IQueryable<Article> articles = context.Articles
                .Include(a => a.Author)
                .Include(c => c.Comments)
                .OrderBy(a => a.Id);

            if (!string.IsNullOrEmpty(name))
                articles = articles.Where(p => p.Author.NickName.Contains(name));

            switch (sortOrder)
            {
                case SortState.NumberOfCommentsAsc:
                    articles = articles.OrderBy(s => s.Comments.Count);
                    break;
                case SortState.NumberOfCommentsDesc:
                    articles = articles.OrderByDescending(s => s.Comments.Count);
                    break;
                case SortState.DateAsc:
                    articles = articles.OrderBy(s => s.CreateTime);
                    break;
                default:
                    articles = articles.OrderByDescending(s => s.CreateTime);
                    break;
            }

            var countArticles = await articles.CountAsync();
            var itemsPerPage =
                await articles.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            return View(new ArticlesIndexViewModel 
            {
                PageViewModel = new PageViewModel(countArticles, page, PageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name),
                Articles = itemsPerPage
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        public IActionResult Create(Article article, string StringTags)
        {
            if (ModelState.IsValid)
            {
                if (StringTags is null)
                {
                    article.Tags = new List<Tag>();
                    foreach (string TagName in StringTags.Split(' '))
                        article.Tags.Add(new Tag { Name = TagName });
                }
                article.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                context.Articles.Add(article);
                context.SaveChanges();
                return RedirectToAction("Show", "Articles", new { article.Id });
            }
            return View(article);
        }

        [HttpGet]
        public IActionResult Show(int ArticleId) 
        {
            Article currentArticle = 
                context.Articles
                .Include(c=>c.Comments)
                    .ThenInclude(a => a.Author)
                .Where(a => a.Id == ArticleId)
                .FirstOrDefault();
            context.Entry(currentArticle).Collection(t => t.Tags);
            context.Entry(currentArticle).Collection(c => c.Comments);
            return View(currentArticle);
        }
            


        /// <summary>
        /// Post method "Show" add new comment.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Show(string CommentText, int ArticleId)
        {
            User CurrentUser = await userManager.GetUserAsync(User); 
            context.Comments.Add(new Comment
            {
                Text = CommentText,
                Author = CurrentUser,
                ArticleId = ArticleId
            });
            context.SaveChanges();
            return RedirectToAction("Show", "Articles", new { ArticleId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int ArticleId) =>
           View(context.Articles.Where(a => a.Id == ArticleId).FirstOrDefault());

        [Authorize]
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
