using ecommerce.Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.Entities.ViewModels
{
    public class UserRoleVM
    {
        public ApplicationUser User { get; set; } = default!;
        [ValidateNever]
        public string Role { get; set; } = string.Empty;

    }
}
