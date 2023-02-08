using System;
using System.Collections.Generic;
using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using BulkyBook.DataAccess.Repositories.OrderHeaderRepository;

namespace BulkyBook.DataAccess.Repositories.OrderRepository
{
	public class OrderHeaderRepository: BaseRepository<ApplicationDbContext, OrderHeader>, IOrderHeaderRepository
	{
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<OrderHeader> _dbSet;
        public CoverTypeRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<OrderHeader>();
        }

        public void UpdateShippingStatus(string trackingNumber, string carrier)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatus(string orderStatus)
        {
            throw new NotImplementedException();
        }
    }
}
