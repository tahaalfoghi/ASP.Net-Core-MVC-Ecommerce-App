using ecommerce.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.DataAccess.Services.Repo
{
    public interface IOrderRepository:IRepository<Order>
    {
        Task UpdateStatus(int id, string orderStatus, string payStatus);
    }
}
