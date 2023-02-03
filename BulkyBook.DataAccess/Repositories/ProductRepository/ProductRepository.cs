using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repositories.ProductRepository
{
    public class ProductRepository : BaseRepository<ApplicationDbContext, Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Product> _dbSet;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Product>();
        }

        public void Update(Product product)
        {
            var productFromDb = _dbSet.FirstOrDefault(x => x.Id == product.Id);
            if(productFromDb != null)
            {
                product.Title = productFromDb.Title;
                product.Description = productFromDb.Description;
                product.CategoryId = productFromDb.CategoryId;
                product.ISBN = productFromDb.ISBN;
                product.Author = productFromDb.Author;
                product.Price = productFromDb.Price;
                product.CoverTypeId = productFromDb.CoverTypeId;

                if(!String.IsNullOrWhiteSpace(product.ImageUrl))
                {
                    productFromDb.ImageUrl= product.ImageUrl;
                }
            }

        }
    }
}
