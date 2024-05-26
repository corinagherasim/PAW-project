using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using proj.Data;
using System.Data;
using Microsoft.EntityFrameworkCore;
using proj.Models;

namespace proj.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<UserCustom> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ApplicationDbContext _context;

        public UsersController(
            ApplicationDbContext context,
            UserManager<UserCustom> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        
        public IActionResult ListUser()
        {
            return View();
        }
    }
}
