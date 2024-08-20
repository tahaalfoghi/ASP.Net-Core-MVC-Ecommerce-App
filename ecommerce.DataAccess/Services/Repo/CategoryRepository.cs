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
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        private readonly AppDbContext context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
