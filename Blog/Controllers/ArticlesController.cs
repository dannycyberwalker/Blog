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
        private readonly int PageSize = 4;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public ArticlesController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">This is string for search an article by author. </param>
        /// <param name="categoryId">
        /// With the default value of the parameter, all articles will be selected.
        /// It must match the value in comparison "categoryId != 1".
        /// </param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string name, int categoryId = 1 ,int page = 1,
            SortState sortOrder = SortState.DateDesc)
        {
            //getting data
            var articles = _context.Articles
                .Include(a => a.Author)
                .Include(c => c.Comments)
                .OrderBy(a => a.Id)
                .Skip((page - 1) * PageSize)
                .Take(PageSize);
            
            //filtering
            if (!string.IsNullOrEmpty(name))
                articles = articles.Where(p => p.Author.NickName.Contains(name));
            else if (categoryId != 1)
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
            

            var countArticles = await _context.Articles.CountAsync();
            
            FilterViewModel fvm = new FilterViewModel(name,
                await _context.Categories.SingleAsync(c => c.Id == categoryId));
            
            return View(new ArticlesIndexViewModel 
            {
                PageViewModel = new PageViewModel(countArticles, page, PageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = fvm,
                Categories = _context.Categories.ToList(),
                Articles = articles.ToList()
            });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create() 
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [Authorize]
        [HttpPost]
        public async  Task<IActionResult> Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _context.Articles.AddAsync(article);
                await _context.SaveChangesAsync();
                return RedirectToAction("Show", "Articles", new { ArticleId = article.Id });
            }
            return View(article);
        }

        [HttpGet]
        public async  Task<ActionResult> Show(int articleId)
        {
            Article currentArticle = await 
                _context.Articles
                .Include(c => c.Category)
                .Include(c => c.Comments)
                    .ThenInclude(a => a.Author)
                .Where(a => a.Id == articleId)
                .SingleAsync();
            return View(currentArticle);
        }

        /// <summary>
        /// Post method "Show" add new comment.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Show(string commentText, int articleId)
        {
            User currentUser = await _userManager.GetUserAsync(User); 
            await _context.Comments.AddAsync(new Comment
            {
                Text = commentText,
                Author = currentUser,
                ArticleId = articleId
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Show", "Articles", new { ArticleId = articleId });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(int articleId)
        {
            Article article = _context.Articles.Single(a => a.Id == articleId);
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value == article.UserId
                || User.IsInRole("admin"))
            {
                ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
                return View(article);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(Article articleEdited)
        {
           Article articleFromDb = _context.Articles.Single(a => a.Id == articleEdited.Id);
            articleFromDb.Headline = articleEdited.Headline;
            articleFromDb.PictureName = articleEdited.PictureName;
            articleFromDb.ShortDescription = articleEdited.ShortDescription;
            articleFromDb.Text = articleEdited.Text;
            articleFromDb.CategoryId = articleEdited.CategoryId;
            _context.SaveChanges();
            return RedirectToAction("Show", "Articles", new { ArticleId = articleEdited.Id });
        }
    }
}
