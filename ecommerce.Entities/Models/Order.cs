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
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ValidateNever]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = default!;
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public decimal TotalAmount { get; set; }

        [AllowedValues("Processing,Shipped,Delivered,Completed,On Hold, Canceled, Refunded, Failed")]
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier {  get; set; }
        public DateTime PaymentDate { get; set; }
        //Stripe props
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        // User
        public string? UserName { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
   
}
