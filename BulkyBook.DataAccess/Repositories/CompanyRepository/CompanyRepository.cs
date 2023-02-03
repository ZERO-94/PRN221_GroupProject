using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.CompanyRepository
{
    public class CompanyRepository : BaseRepository<ApplicationDbContext, Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Company> _dbSet;
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<Company>();
        }

        public void Update(Company company)
        {
            _dbContext.Update(company);
        }
    }
}
