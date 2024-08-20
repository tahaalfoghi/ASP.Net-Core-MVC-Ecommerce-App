using ecommerce.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Entities.ViewModels
{
	public class ShoppingCartVM
	{
		[ValidateNever]
		public IEnumerable<ShoppingCart> CartItemsList { get; set; } = default!;
		public decimal Total { get; set; }
		[ValidateNever]
		public Order Order { get; set; } = default!;

	}
}
