using System;
using BulkyBook.DataAccess.Repositories.BaseRepository;
using BulkyBook.BusinessObject.Models;

namespace BulkyBook.DataAccess.Repositories.OrderHeaderRepository
{
	public interface IOrderHeaderRepository: IBaseRepository<OrderHeader>	{
		public void UpdateShippingStatus(string trackingNumber, string carrier);
		public void UpdateStatus(string orderStatus);
	}
}

