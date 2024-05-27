using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using proj.Models;

namespace proj.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserCustom>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<UserCustom> UsersCustom { get; set; }
        public DbSet<UserSuggestionModel> UserSuggestions { get; set; }

    }
}