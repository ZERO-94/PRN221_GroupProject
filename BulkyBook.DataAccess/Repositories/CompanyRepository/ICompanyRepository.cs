using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.CompanyRepository
{
    public interface ICompanyRepository: IBaseRepository<Company>
    {
        public void Update(Company company);
    }
}
