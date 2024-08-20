using ecommerce.DataAccess.Data;
using ecommerce.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.DataAccess.Services.Repo
{
    public class ShoppingCartRepository : RepositoryBase<ShoppingCart>, IShoppingCartRepository
    {
        private readonly AppDbContext context;
        public ShoppingCartRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }

        public int IncreaseCount(ShoppingCart cart, int count)
        {
            cart.Count += count;
            return cart.Count;
        }
        public int DecreaseCount(ShoppingCart cart, int count)
        {
            cart.Count -= count;
            return cart.Count;
        }
        public async Task DeleteRange(IEnumerable<ShoppingCart> carts)
        {
            if (carts is not null)
                context.ShoppingCarts.RemoveRange(carts);

        }
       

    }
}
