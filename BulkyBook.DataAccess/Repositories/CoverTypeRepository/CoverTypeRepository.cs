using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.CoverTypeRepository
{
    public class CoverTypeRepository : BaseRepository<ApplicationDbContext, CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<CoverType> _dbSet;
        public CoverTypeRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<CoverType>();
        }

        public void Update(CoverType coverType)
        {
            _dbSet.Update(coverType);
        }
    }
}
