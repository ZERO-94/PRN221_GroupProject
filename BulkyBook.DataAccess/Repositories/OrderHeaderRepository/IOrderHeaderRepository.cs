using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.BusinessObject.Models;
using BulkyBook.DataAccess.Repositories.BaseRepository;

namespace BulkyBook.DataAccess.Repositories.OrderHeaderRepository
{
	public interface IOrderHeaderRepository : IBaseRepository<OrderHeader>
	{
	}
}
