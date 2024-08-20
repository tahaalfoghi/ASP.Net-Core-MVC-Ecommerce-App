using ecommerce.DataAccess.Services.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.DataAccess.Services.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        IUserRepository UserRepository { get; }
        Task CommitAsync();
        
    }
}
