using BulkyBook.DataAccess.Repositories.ApplicationUserRepository;
using BulkyBook.DataAccess.Repositories.CategoryRepository;
using BulkyBook.DataAccess.Repositories.CoverTypeRepository;
using BulkyBook.DataAccess.Repositories.OrderDetailRepository;
using BulkyBook.DataAccess.Repositories.OrderHeaderRepository;
using BulkyBook.DataAccess.Repositories.ProductRepository;

namespace BulkyBook.DataAccess.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
        }

        private ICategoryRepository categoryRepository;
        private ICoverTypeRepository coverTypeRepository;
        private IProductRepository productRepository;
        private IApplicationUserRepository applicationUserRepository;
        private IOrderDetailRepository orderDetailRepository;
        private IOrderHeaderRepository orderHeaderRepository;

        public ICategoryRepository CategoryRepository { 
            get
            {
                if(this.categoryRepository == null)
                {
                    this.categoryRepository = new CategoryRepository(context);
                }
                return this.categoryRepository;
            } 
        }

        public ICoverTypeRepository CoverTypeRepository { 
            get 
            {
                if(this.coverTypeRepository == null)
                {
                    this.coverTypeRepository = new CoverTypeRepository(context);
                }
                return this.coverTypeRepository;
            } 
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if(this.productRepository == null)
                {
                    this.productRepository = new ProductRepository(context);
                }
                return this.productRepository;
            }
        }

        public IApplicationUserRepository ApplicationUserRepository
        {
            get
            {
                if(this.applicationUserRepository == null)
                {
                    this.applicationUserRepository = new ApplicationUserRepository(context);
                }    
                return this.applicationUserRepository;
            }
        }   
        public IOrderDetailRepository OrderDetailRepository
		{
            get
			{
                if (this.orderDetailRepository == null)
				{
                    this.orderDetailRepository = new OrderDetailRepository(context);
				}
                return this.orderDetailRepository;
			}
		}

		public IOrderHeaderRepository OrderHeaderRepository
		{
            get
			{
                if (this.orderHeaderRepository == null)
				{
                    this.orderHeaderRepository = new OrderHeaderRepository(context);
				}
                return this.orderHeaderRepository;
			}
		}

		public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
