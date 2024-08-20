using ecommerce.DataAccess.Data;
using ecommerce.DataAccess.Services.UnitOfWork;
using ecommerce.Entities.Models;
using ecommerce.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ecommerce.web.Helpers;
using Stripe.Checkout;
using Microsoft.Extensions.Options;

namespace ecommerce.web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]

    public class CartController : Controller
    {
        private readonly IUnitOfWork uow;
        private readonly AppDbContext context;
        public CartController(IUnitOfWork uow, AppDbContext context)
        {
            this.uow = uow;
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            var ShoppingCartVM = new ShoppingCartVM()
            {
                CartItemsList = await uow.ShoppingCartRepository.GetAllByFilterAsync(x => x.UserId == claim.Value && x.Count > 0, includes: "Product,Product.Category")
            };
            var count = context.Set<ShoppingCart>().Where(x => x.UserId == claim.Value).ToList().Count();
            HttpContext.Session.SetInt32(CartCount.sessionCart, count);

            foreach (var item in ShoppingCartVM.CartItemsList)
            {
                ShoppingCartVM.Total += (item.Product.Price * item.Count);
            }

            return View(ShoppingCartVM);
        }
        public async Task<IActionResult> Plus(int cartId)
        {
            var existCart = await uow.ShoppingCartRepository.GetAsync(x => x.Id == cartId);

            uow.ShoppingCartRepository.IncreaseCount(existCart, 1);
            await uow.CommitAsync();
            var count = context.Set<ShoppingCart>().Where(x => x.UserId == existCart.UserId).ToList().Count();
            HttpContext.Session.SetInt32(CartCount.sessionCart, count + 1);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Minus(int cartId)
        {
            var existCart = await uow.ShoppingCartRepository.GetAsync(x => x.Id == cartId);

            if (existCart.Count <= 0)
            {
                await uow.ShoppingCartRepository.DeleteAsync(existCart);
                await uow.CommitAsync();
                var count = context.Set<ShoppingCart>().Where(x => x.UserId == existCart.UserId).ToList().Count();
                HttpContext.Session.SetInt32(CartCount.sessionCart, count - 1);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                uow.ShoppingCartRepository.DecreaseCount(existCart, 1);
                var count = context.Set<ShoppingCart>().Where(x => x.UserId == existCart.UserId).ToList().Count();
                HttpContext.Session.SetInt32(CartCount.sessionCart, count - 1);
            }
            await uow.CommitAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int cartId)
        {

            var existsCart = await uow.ShoppingCartRepository.GetAsync(x => x.Id == cartId);
            await uow.ShoppingCartRepository.DeleteAsync(existsCart);
            await uow.CommitAsync();
            var count = context.Set<ShoppingCart>().Where(x => x.UserId == existsCart.UserId).ToList().Count();
            HttpContext.Session.SetInt32(CartCount.sessionCart, count);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Summery()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var ShoppingCartVM = new ShoppingCartVM()
            {
                CartItemsList = await uow.ShoppingCartRepository.GetAllByFilterAsync(x => x.UserId == claim.Value && x.Count > 0, includes: "Product,Product.Category"),
                Order = new()
            };

            ShoppingCartVM.Order.User = await uow.UserRepository.GetAsync(x => x.Id == claim.Value);

            ShoppingCartVM.Order.UserName = ShoppingCartVM.Order.User.FirstName;
            ShoppingCartVM.Order.PhoneNumber = ShoppingCartVM.Order.User.PhoneNumber;
            ShoppingCartVM.Order.City = ShoppingCartVM.Order.User.City;

            foreach (var item in ShoppingCartVM.CartItemsList)
            {
                ShoppingCartVM.Order.TotalAmount += (item.Product.Price * item.Count);
            }

            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(ShoppingCartVM cartVM)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            cartVM.CartItemsList = await uow.ShoppingCartRepository.GetAllByFilterAsync(x => x.UserId == claim.Value, includes: "Product,Product.Category");

            cartVM.Order.OrderDate = DateTime.Now;
            cartVM.Order.PaymentDate = DateTime.Now;
            cartVM.Order.Status = SD.Pending;
            cartVM.Order.PaymentStatus = SD.Pending;
            cartVM.Order.UserId = claim.Value;


            foreach (var item in cartVM.CartItemsList)
            {
                cartVM.Order.TotalAmount += (item.Product.Price * item.Count);
            }

            await uow.OrderRepository.CreateAsync(cartVM.Order);
            await uow.CommitAsync();

            foreach (var item in cartVM.CartItemsList)
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderId = cartVM.Order.Id,
                    Price = item.Product.Price,
                    Count = item.Count
                };
                await uow.OrderDetailRepository.CreateAsync(orderDetail);
                await uow.CommitAsync();
            }

            var domain = "https://localhost:7208/";

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/cart/orderconfirmation?id={cartVM.Order.Id}",
                CancelUrl = domain + $"customer/cart/index"
            };

            foreach(var item in cartVM.CartItemsList)
            {
                var sessionLineOption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price*100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineOption);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            cartVM.Order.SessionId = session.Id;
            await uow.CommitAsync();

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
            /*return RedirectToAction("Index");*/
        }
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await uow.OrderRepository.GetAsync(x =>x.Id == id);
            
            var service = new SessionService();
            Session session = service.Get(order.SessionId);
            order.PaymentIntentId = session.Id;
            if(session.PaymentStatus.ToLower() == "paid")
            {
                await uow.OrderRepository.UpdateStatus(order.Id, SD.Approved, SD.Approved);
                await uow.CommitAsync();
            }

            var userCartItems = await uow.ShoppingCartRepository.GetAllByFilterAsync(x => x.UserId == order.UserId);
            await uow.ShoppingCartRepository.DeleteRange(userCartItems);
            await uow.CommitAsync();
            return View(id);
        }
    }
}
