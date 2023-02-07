using BulkyBook.DataAccess.Repositories.ApplicationUserRepository;
using BulkyBook.DataAccess.Repositories.CategoryRepository;
using BulkyBook.DataAccess.Repositories.CoverTypeRepository;
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

        public async Task<int> SaveAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
