
using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.CategoryRepository
{
    public class CategoryRepository : BaseRepository<ApplicationDbContext, Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Category> _dbSet;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Category>();
        }

        public void Update(Category category)
        {
            _dbSet.Update(category);
        }

    }
}
