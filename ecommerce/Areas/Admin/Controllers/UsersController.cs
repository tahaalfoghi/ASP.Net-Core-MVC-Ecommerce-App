using ecommerce.DataAccess.Data;
using ecommerce.DataAccess.Services.UnitOfWork;
using ecommerce.Entities.Models;
using ecommerce.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ecommerce.web.Helpers;

namespace ecommerce.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    
    public class UsersController : Controller
    {
        private readonly AppDbContext context;
        private readonly IUnitOfWork uow;
        public UsersController(AppDbContext context, IUnitOfWork uow)
        {
            this.context = context;
            this.uow = uow;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claims.Value;

                        

            return View(context.Set<ApplicationUser>().Where(x => x.Id != userId).ToList());
        }
        public async Task<IActionResult> LockUnlock(string? userId)
        {
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(x=>x.Id == userId);
            if(user is null)
            {
                return NotFound();
            }
            if(user.LockoutEnd is null || user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddMinutes(10);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            context.SaveChanges();
            return RedirectToAction("Index","Users", new {area="Admin"});
        }
        
    }
}
