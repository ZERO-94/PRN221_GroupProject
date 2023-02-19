using BulkyBook.BusinessObject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

        }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<CoverType> CoverTypes => Set<CoverType>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public DbSet<OrderHeader> OrderHeaders => Set<OrderHeader>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    }
}
