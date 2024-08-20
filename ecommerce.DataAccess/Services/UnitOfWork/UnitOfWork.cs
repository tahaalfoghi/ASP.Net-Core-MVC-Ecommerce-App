using ecommerce.DataAccess.Data;
using ecommerce.DataAccess.Services.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.DataAccess.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext context;
        public ICategoryRepository CategoryRepository { get;}
        public IProductRepository ProductRepository { get;}
        public IShoppingCartRepository ShoppingCartRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IOrderDetailRepository OrderDetailRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public UnitOfWork(AppDbContext context, ICategoryRepository CategoryRepository, IProductRepository ProductRepository, IShoppingCartRepository ShoppingCartRepository, IOrderRepository OrderRepository, IOrderDetailRepository OrderDetailRepository, IUserRepository UserRepository)
        {
            this.context = context;
            this.CategoryRepository = CategoryRepository;
            this.ProductRepository = ProductRepository;
            this.ShoppingCartRepository = ShoppingCartRepository;
            this.OrderRepository = OrderRepository;
            this.OrderDetailRepository = OrderDetailRepository;
            this.UserRepository = UserRepository;
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
