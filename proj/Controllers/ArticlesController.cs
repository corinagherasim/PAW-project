using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proj.Data;
using proj.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace proj.Controllers
{
    [Authorize(Roles = "Editor")]
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

        // GET: Articles/Create
        [HttpGet]
        public IActionResult Create(int? suggestionId)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "CategoryName");

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

                    ViewBag.SuggestionId = suggestionId;

                    return View(article);
                }
            }

            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,HeadLine,Photo,CategoryId")] ArticleModel article, int? suggestionId)
        {
            /*if (ModelState.IsValid) // nu imi trece de asta
            {*/
            try
            {
                article.Date = DateTime.Now;
                _context.Add(article);
                await _context.SaveChangesAsync();

                if (suggestionId.HasValue)
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
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "CategoryName", article.CategoryId);
            return View(article);
        }



    }
}
