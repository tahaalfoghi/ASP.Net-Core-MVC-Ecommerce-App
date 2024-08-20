using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Entities.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]

        public Product Product { get; set; } = default!;
        [Range(1, 50, ErrorMessage = "product count can be from 1 to 50 only")]
        public int Count { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; } = default!;
       
    }
}
