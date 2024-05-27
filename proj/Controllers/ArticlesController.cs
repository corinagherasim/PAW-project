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
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,HeadLine,Photo,CategoryId")] ArticleModel article)
        {
            /*if (ModelState.IsValid) // nu imi trece de asta
            {*/
                try
                {
                    article.Date = DateTime.Now;
                    _context.Add(article);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
            }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving article to the database");
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
          /*  }*/
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", article.CategoryId);
            return View(article);
        }

    }
}