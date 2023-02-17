using BulkyBook.DataAccess.Repositories.ApplicationUserRepository;
using BulkyBook.DataAccess.Repositories.CategoryRepository;
using BulkyBook.DataAccess.Repositories.CoverTypeRepository;
using BulkyBook.DataAccess.Repositories.OrderDetailRepository;
using BulkyBook.DataAccess.Repositories.OrderHeaderRepository;
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
        public IOrderDetailRepository OrderDetailRepository { get; }
        public IOrderHeaderRepository OrderHeaderRepository { get; }
        public Task<int> SaveAsync();
    }
}
