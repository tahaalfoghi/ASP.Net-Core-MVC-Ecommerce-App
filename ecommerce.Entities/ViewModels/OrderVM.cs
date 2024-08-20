using ecommerce.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Entities.ViewModels
{
    public class OrderVM
    {
        [ValidateNever]
        public Order Order { get; set; }
        [ValidateNever]
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
