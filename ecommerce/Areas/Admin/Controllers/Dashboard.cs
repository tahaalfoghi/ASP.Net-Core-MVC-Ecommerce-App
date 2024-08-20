using ecommerce.DataAccess.Services.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class Dashboard : Controller
    {
        private readonly IUnitOfWork uow;

        public Dashboard(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Orders = (await uow.OrderRepository.GetAllAsync()).Count();
            ViewBag.Products = (await uow.ProductRepository.GetAllAsync()).Count();
            ViewBag.Users = (await uow.UserRepository.GetAllAsync()).Count();
            ViewBag.DeliverdOrders = (await uow.OrderRepository.GetAllByFilterAsync(x => x.Status == "Delivered")).Count();
            return View();
        }
    }
}
