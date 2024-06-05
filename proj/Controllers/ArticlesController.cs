using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using proj.Data;
using proj.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace proj.Controllers
{
    
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserCustom> _userManager;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(ApplicationDbContext context, UserManager<UserCustom> userManager, ILogger<ArticlesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Details(int id)
        {
            var article = await _context.Articles
                .Include(a => a.Comments)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
        [Authorize(Roles = "User,Admin,Editor")]
        [HttpPost]
        public async Task<IActionResult> AddComment(int articleId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction("Details", new { id = articleId });
            }

            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Anonymous";

            var comment = new CommentModel
            {
                Content = content,
                Date = DateTime.Now,
                ArticleId = articleId,
                UserName = userName
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = articleId });
        }

        public async Task<IActionResult> Category(int id, string sortOrder)
        {
            var category = await _context.Categories
                .Include(c => c.Articles)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            ViewBag.TitleSortParm = sortOrder == "title" ? "title_desc" : "title";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date";
            ViewBag.CommentSortParm = sortOrder == "comments" ? "comments_desc" : "comments";

            var articles = from a in category.Articles
                           select a;

            switch (sortOrder)
            {
                case "title":
                    articles = articles.OrderBy(a => a.Title);
                    break;
                case "title_desc":
                    articles = articles.OrderByDescending(a => a.Title);
                    break;
                case "date":
                    articles = articles.OrderByDescending(a => a.Date).ThenByDescending(a => a.Id);
                    break;
                case "date_desc":
                    articles = articles.OrderBy(a => a.Date).ThenBy(a => a.Id);
                    break;
                case "comments":
                    articles = articles.OrderByDescending(a => a.Comments?.Count ?? 0)
                                      .ThenByDescending(a => a.Date)
                                      .ThenByDescending(a => a.Id);
                    break;
                case "comments_desc":
                    articles = articles.OrderBy(a => a.Comments?.Count ?? 0); // Sortare inversă după numărul de comentarii
                    break;
                default:
                    articles = articles.OrderByDescending(a => a.Comments.Count).ThenByDescending(a => a.Date).ThenByDescending(a => a.Id);
                    break;
            }

            category.Articles = articles.ToList();

            return View(category);
        }

        [Authorize(Roles = "Editor")]
        public IActionResult Create(int? suggestionId)
        {
            if (suggestionId != null)
            {
                var suggestion = _context.UserSuggestions.FirstOrDefault(s => s.Id == suggestionId);
                if (suggestion != null)
                {
                    var article = new ArticleModel
                    {
                        Title = suggestion.Title,
                        Content = suggestion.Content
                    };

                    var categories = _context.Categories.ToList();
                    categories.Insert(0, new CategoryModel { Id = 0, CategoryName = "Other" });
                    ViewData["Categories"] = new SelectList(categories, "Id", "CategoryName");
                    ViewBag.SuggestionId = suggestionId;
                    return View(article);
                }
            }

            var allCategories = _context.Categories.ToList();
            allCategories.Insert(0, new CategoryModel { Id = 0, CategoryName = "Other" });
            ViewData["Categories"] = new SelectList(allCategories, "Id", "CategoryName");

            return View();
        }

        [Authorize(Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,HeadLine,Photo,CategoryId,IsExternal,ExternalLink")] ArticleModel article, string NewCategory)
        {
            if (article.IsExternal)
            {
                article.Content = "Această știre a fost preluată de pe alt site.";
            }

            if (!string.IsNullOrEmpty(NewCategory))
            {
                // Adaugă noua categorie în baza de date
                var newCategory = new CategoryModel { CategoryName = NewCategory };
                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();

                // Setează categoria articolului la noua categorie
                article.CategoryId = newCategory.Id;
            }

            /*if (ModelState.IsValid)
            {*/
                try
                {
                    article.Date = DateTime.Now;
                    _context.Add(article);
                    await _context.SaveChangesAsync();

                    // Șterge sugestia utilizată
                    if (int.TryParse(Request.Form["suggestionId"], out int suggestionId))
                    {
                        var suggestion = await _context.UserSuggestions.FindAsync(suggestionId);
                        if (suggestion != null)
                        {
                            _context.UserSuggestions.Remove(suggestion);
                            await _context.SaveChangesAsync();
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving article to the database");
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            /*}*/

            var allCategories = _context.Categories.ToList();
            allCategories.Insert(0, new CategoryModel { Id = 0, CategoryName = "Other" });
            ViewData["Categories"] = new SelectList(allCategories, "Id", "CategoryName", article.CategoryId);

            return View(article);
        }

        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(new List<ArticleModel>());
            }

            var articles = await _context.Articles
                                         .Include(a => a.Comments)
                                         .Where(a => a.Title.Contains(query) || a.Content.Contains(query) || a.HeadLine.Contains(query))
                                         .ToListAsync();

            return View(articles);
        }

    }
}
