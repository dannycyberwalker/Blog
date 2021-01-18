using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Blog.Models;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">This is string for search an article by author. </param>
        /// <param name="categoryId">
        /// With the default value of the parameter, all articles will be selected.
        /// It must match the value in comparison "categoryId != 2".
        /// </param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string name, int categoryId = 2 ,int page = 1,
            SortState sortOrder = SortState.DateDesc)
        {
            //getting data
            var articles = context.Articles
                .Include(a => a.Author)
                .Include(c => c.Comments)
                .OrderBy(a => a.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);
            
            //filtering
            if (!string.IsNullOrEmpty(name))
                articles = articles.Where(p => p.Author.NickName.Contains(name));
            else if (categoryId != 2)
                articles = articles.Where(c => c.CategoryId == categoryId);
            else
            {
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
            }
            

            var countArticles = await articles.CountAsync();
            
            FilterViewModel fvm = new FilterViewModel(name,
                await (context.Categories).SingleAsync(c => c.Id == categoryId));
            
            return View(new ArticlesIndexViewModel 
            {
                PageViewModel = new PageViewModel(countArticles, page, PageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = fvm,
                Categories = context.Categories.ToList(),
                Articles = articles.ToList()
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create() 
        {
            ViewBag.Categories = new SelectList(context.Categories, "Id", "Name");
            return View();
        }

        [Authorize]
        [HttpPost]
        public async  Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await context.Articles.AddAsync(article);
                await context.SaveChangesAsync();
                return RedirectToAction("Show", "Articles", new { ArticleId = article.Id });
            }
            return View(article);
        }

        [HttpGet]
        public async  Task<ActionResult> Show(int ArticleId)
        {
            Article currentArticle = await 
                context.Articles
                .Include(c => c.Category)
                .Include(c => c.Comments)
                    .ThenInclude(a => a.Author)
                .Where(a => a.Id == ArticleId)
                .SingleAsync();
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
            await context.Comments.AddAsync(new Comment
            {
                Text = CommentText,
                Author = CurrentUser,
                ArticleId = ArticleId
            });
            await context.SaveChangesAsync();
            return RedirectToAction("Show", "Articles", new { ArticleId = ArticleId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int ArticleId)
        {
            Article article = context.Articles.Single(a => a.Id == ArticleId);
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value == article.UserId
                || User.IsInRole("admin"))
            {
                ViewBag.Categories = new SelectList(context.Categories, "Id", "Name");
                return View(article);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Article ArticleEdited)
        {
           Article ArticleFromDb = context.Articles.Single(a => a.Id == ArticleEdited.Id);
            ArticleFromDb.Headline = ArticleEdited.Headline;
            ArticleFromDb.PictureName = ArticleEdited.PictureName;
            ArticleFromDb.ShortDescription = ArticleEdited.ShortDescription;
            ArticleFromDb.Text = ArticleEdited.Text;
            ArticleFromDb.CategoryId = ArticleEdited.CategoryId;
            context.SaveChanges();
            return RedirectToAction("Show", "Articles", new { ArticleId = ArticleEdited.Id });
        }
    }
}
