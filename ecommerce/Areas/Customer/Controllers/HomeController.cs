using ecommerce.DataAccess.Services.UnitOfWork;
using ecommerce.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList.Extensions;

namespace ecommerce.web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork uow;

        public HomeController(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 8;

            var list = (await uow.ProductRepository.GetAllAsync(includes: "Category")).ToPagedList(pageNumber, pageSize);
            return View(list);
        }

        public async Task<IActionResult> AddToCart(int id)
        {

            var cart = new ShoppingCart()
            {
                ProductId = id,
                Product = await uow.ProductRepository.GetAsync(x=>x.Id == id, includes:"Category"),
                Count = 1
            };
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.UserId = claims.Value;

            var existsCart = await uow.ShoppingCartRepository
                                     .GetAsync(x=>x.Id== cart.Id && x.UserId == claims.Value && x.ProductId == cart.ProductId);
           
            if(existsCart is null)
            {
                cart.Id = 0;
                await uow.ShoppingCartRepository.CreateAsync(cart);
                TempData["Create"] = "Item Added to cart successfully";

            }
            else
            {
                uow.ShoppingCartRepository.IncreaseCount(existsCart, cart.Count);
            }

            await uow.CommitAsync();

            return RedirectToAction("Index");
        }

    }
}
