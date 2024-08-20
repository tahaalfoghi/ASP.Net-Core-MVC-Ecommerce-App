using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } =  string.Empty;
        public string? Description { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
