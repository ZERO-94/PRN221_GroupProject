using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repositories.OrderDetailRepository
{
	public class OrderDetailRepository : BaseRepository<ApplicationDbContext, OrderDetail>, IOrderDetailRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly DbSet<OrderDetail> _dbSet;
		public OrderDetailRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
			_dbSet = context.Set<OrderDetail>();
		}
	}
}
