using BulkyBook.BusinessObject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<CoverType> CoverTypes => Set<CoverType>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public DbSet<Company> Companies => Set<Company>();
    }
}
