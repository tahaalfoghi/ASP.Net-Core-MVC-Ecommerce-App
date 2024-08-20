using ecommerce.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.DataAccess.Services.Repo
{
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    {
        int IncreaseCount(ShoppingCart cart, int count);
        int DecreaseCount(ShoppingCart cart, int count);
        Task DeleteRange(IEnumerable<ShoppingCart> carts);
    }
}
