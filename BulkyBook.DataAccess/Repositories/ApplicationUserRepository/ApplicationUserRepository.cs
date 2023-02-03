using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.ApplicationUserRepository
{
    public class ApplicationUserRepository : BaseRepository<ApplicationDbContext, ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
