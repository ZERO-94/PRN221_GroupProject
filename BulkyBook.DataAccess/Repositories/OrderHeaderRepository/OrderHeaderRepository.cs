using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repositories.OrderHeaderRepository
{
	public class OrderHeaderRepository : BaseRepository<ApplicationDbContext, OrderHeader>, IOrderHeaderRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly DbSet<OrderHeader> _dbSet;
		public OrderHeaderRepository(ApplicationDbContext context) : base(context)
		{
			this._context = context;
			_dbSet = context.Set<OrderHeader>();
		}
		public void Update(OrderHeader orderHeader)
		{
			_dbSet.Update(orderHeader);
		}
	}
}
