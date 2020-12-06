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

namespace Blog.Controllers
{
    public class ArticlesController : Controller
    {
        private ApplicationDbContext context;
        private UserManager<User> userManager;
        public int PageSize = 4;
        public ArticlesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult List(int PageId = 1)
        {
            List<ListViewModel> models = new List<ListViewModel>();
            string userId = userManager.GetUserId(User);
            foreach (var article in context.Articles)
            {
                models.Add(new ListViewModel
                {
                    Article = article,
                    CountComments = context.Comments.Where(c => c.ArticleId == article.Id).Count(),
                    AuthorName = context.Users.FirstOrDefault(n => n.Id == article.UserId).NickName,
                    AuthorId = article.UserId,
                    UserId = userId
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

        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize]
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
                ClaimsPrincipal currentUser = User;
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                article.UserId = currentUserId;

                context.Articles.Add(article);
                context.SaveChanges();
            }
            return View();
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
