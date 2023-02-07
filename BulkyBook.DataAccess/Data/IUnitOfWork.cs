using BulkyBook.DataAccess.Repositories.ApplicationUserRepository;
using BulkyBook.DataAccess.Repositories.CategoryRepository;
using BulkyBook.DataAccess.Repositories.CoverTypeRepository;
using BulkyBook.DataAccess.Repositories.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Data
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public ICoverTypeRepository CoverTypeRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IApplicationUserRepository ApplicationUserRepository { get; }
        public Task<int> SaveAsync();
    }
}
