using ecommerce.DataAccess.Services.UnitOfWork;
using ecommerce.Entities.ViewModels;
using ecommerce.web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ecommerce.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork uow;
        
        public OrderController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await uow.OrderRepository.GetAllAsync(includes:"User");

            return Json(new {data= orders});
        }
        public async Task<IActionResult> Details(int orderId)
        {
            var orderVM = new OrderVM()
            {
                Order = await uow.OrderRepository.GetAsync(x=>x.Id == orderId, "User"),
                OrderDetails = await uow.OrderDetailRepository.GetAllByFilterAsync(x=>x.OrderId == orderId, "Order,Product")
            };

            return View(orderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetails(OrderVM orderVM)
        {
            var order = await uow.OrderRepository.GetAsync(x => x.Id == orderVM.Order.Id);
            order.UserName = orderVM.Order.UserName;
            order.PhoneNumber = orderVM.Order.PhoneNumber;
            order.City = orderVM.Order.City;
            order.Address = orderVM.Order.Address;

            if(orderVM.Order.Carrier is not null)
            {
                order.Carrier = orderVM.Order.Carrier;
            }
            if(orderVM.Order.TrackingNumber is not null)
            {
                order.TrackingNumber = orderVM.Order.TrackingNumber;
            }
            await uow.OrderRepository.UpdateAsync(order);
            await uow.CommitAsync();
            TempData["Update"] = "Order updated successfully";
            return RedirectToAction("Details", "Order", new {orderId= order.Id});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProccessOrder(OrderVM orderVM)
        {
            await uow.OrderRepository.UpdateStatus(orderVM.Order.Id, SD.Proccess, null!);
            await uow.CommitAsync();
            TempData["Update"] = "Order Proccessed successfully";
            return RedirectToAction("Details", "Order", new { orderId = orderVM.Order.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(OrderVM orderVM)
        {
            var existingOrder = await uow.OrderRepository.GetAsync(x => x.Id == orderVM.Order.Id);

            if(existingOrder.PaymentStatus == SD.Approved)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = existingOrder.PaymentIntentId,
                };
                var service = new RefundService();
                Refund refund = await service.CreateAsync(option);
                await uow.OrderRepository.UpdateStatus(existingOrder.Id, SD.Canceled, SD.Refunded);
            }
            else
            {
                await uow.OrderRepository.UpdateStatus(existingOrder.Id, SD.Canceled, SD.Canceled);

            }
            await uow.CommitAsync();

            TempData["Update"] = "Order ha canceled successfully";
            return RedirectToAction("Details", "Order", new { orderId = orderVM.Order.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShipOrder(OrderVM orderVM)
        {
            var order = await uow.OrderRepository.GetAsync(x=>x.Id == orderVM.Order.Id);
            if(orderVM.Order.TrackingNumber is null || orderVM.Order.Carrier is null)
            {
                throw new Exception("ERROR IN OrderController ShipOrder endpoint\r\n Tracking Number or Carrier have null values");
            }
            order.Carrier = orderVM.Order.Carrier;
            order.TrackingNumber = orderVM.Order.TrackingNumber;
            order.ShipDate =  DateTime.Now; 
            await uow.OrderRepository.UpdateStatus(order.Id,SD.Shipped,SD.Paid);
            await uow.OrderRepository.UpdateAsync(order);
            await uow.CommitAsync();
            TempData["Update"] = "Order Shipped successfully";
            return RedirectToAction("Details", "Order",new {orderId = order.Id});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeliverOrder(OrderVM orderVM)
        {
            await uow.OrderRepository.UpdateStatus(orderVM.Order.Id, SD.Delivered, SD.Paid);
            await uow.CommitAsync();
            TempData["Update"] = "Order Delivered successfully";
            return RedirectToAction("Details", "Order",new {orderId = orderVM.Order.Id});
        }
    }
}
