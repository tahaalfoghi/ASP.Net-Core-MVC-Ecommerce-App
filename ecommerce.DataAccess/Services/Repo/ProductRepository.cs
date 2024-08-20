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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly AppDbContext context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
