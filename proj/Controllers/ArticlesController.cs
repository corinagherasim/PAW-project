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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,HeadLine,Photo,CategoryId")] ArticleModel article, string NewCategory)
        {
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
           /* }*/

            var allCategories = _context.Categories.ToList();
            allCategories.Insert(0, new CategoryModel { Id = 0, CategoryName = "Other" });
            ViewData["Categories"] = new SelectList(allCategories, "Id", "CategoryName", article.CategoryId);

            return View(article);
        }





    }
}
