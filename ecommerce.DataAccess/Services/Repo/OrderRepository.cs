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
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly AppDbContext context;
        public OrderRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }
        

        public async Task UpdateStatus(int id, string orderStatus, string payStatus)
        {
            var order = await context.Orders.FirstOrDefaultAsync(x=>x.Id == id);
            if (order is not null)
            {
                if(orderStatus is not null)
                {
                    order.Status = orderStatus;
                }
                if(payStatus is not null)
                {
                    order.PaymentStatus = payStatus;
                    order.PaymentDate = DateTime.Now;
                }
            }
        }
    }
}
