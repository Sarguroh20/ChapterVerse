using ChapterVerseUI.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChapterVerseUI.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class AdminOperationsController : Controller
    {
        private readonly IUserOrderRepo _userOrderRepo;

        public AdminOperationsController (IUserOrderRepo userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _userOrderRepo.UserOrders(true);
            return View(orders);
        }
        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _userOrderRepo.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction(nameof(AllOrders));
        }
        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _userOrderRepo.GetOrderById(orderId);
            if(order == null)
            {
                throw new InvalidOperationException($"Order with id: {orderId} does not found");
            }
            var orderStatusList = (await _userOrderRepo.GetOrderStatuses()).Select(orderStatus =>
            {
                return new SelectListItem
                {
                    Value = orderStatus.Id.ToString(),
                    Text = orderStatus.StatusName,
                    Selected = order.OrderStatusId == orderStatus.Id
                };
            });
            var data = new UpdateOrderStatusModel
            {
                OrderId = orderId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusModel data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    data.OrderStatusList = (
                        await _userOrderRepo.GetOrderStatuses()).Select
                        (OrderStatus =>
                        {
                            return new SelectListItem
                            {
                                Value = OrderStatus.Id.ToString(),
                                Text = OrderStatus.StatusName,
                                Selected = OrderStatus.Id == data.OrderStatusId
                            };
                        });
                    return View(data);
                }
                await _userOrderRepo.ChangeOrderStatus(data);
                TempData["msg"] = "Updated successfully";
            }
            catch(Exception ex)
            {
                TempData["msg"] = "Something went wrong";
            }
            return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = data.OrderId });
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
