using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj.Data;
using proj.Models;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace proj.Controllers
{
    public class UserSuggestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserSuggestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddSuggestions()
        {
            return View();
        }

        // POST: UserSuggestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] UserSuggestionModel userSuggestionModel)
        {
            if (ModelState.IsValid)
            {
                // Set the Date property before adding to the database
                userSuggestionModel.Date = DateTime.Now;

                // Add the suggestion to the database
                _context.Add(userSuggestionModel);
                await _context.SaveChangesAsync();

                // Redirect to a success page or action
                return RedirectToAction(nameof(SuggestionAdded));
            }
            return View(userSuggestionModel);
        }

        // GET: UserSuggestions/SuggestionAdded
        public IActionResult SuggestionAdded()
        {
            return View();
        }

        [Authorize(Roles = "Editor")]
        public async Task<IActionResult> SeeSuggestions()
        {
            var suggestions = await _context.UserSuggestions.ToListAsync();
            return View(suggestions);
        }
        [Authorize(Roles = "Editor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var suggestion = await _context.UserSuggestions.FindAsync(id);
            if (suggestion == null)
            {
                return NotFound();
            }

            _context.UserSuggestions.Remove(suggestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(SeeSuggestions));
        }

    }
}

/*
        // GET: UserSuggestions
        public async Task<IActionResult> Index()
        {
            var suggestions = await _context.UserSuggestions.ToListAsync();
            return View(suggestions);
        }

        // GET: UserSuggestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserSuggestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] UserSuggestionModel suggestion)
        {
            if (ModelState.IsValid)
            {
                suggestion.Date = DateTime.Now;
                _context.Add(suggestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suggestion);
        }

        // GET: UserSuggestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suggestion = await _context.UserSuggestions.FirstOrDefaultAsync(m => m.Id == id);
            if (suggestion == null)
            {
                return NotFound();
            }

            return View(suggestion);
        }

        // POST: UserSuggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suggestion = await _context.UserSuggestions.FindAsync(id);
            _context.UserSuggestions.Remove(suggestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

*/