using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> CheckOut()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;

            return View(new CheckOutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutInfoInput checkOutInfoInput)
        {
            var orderSuspend = await _orderService.SuspendOrder(checkOutInfoInput);
            if (!orderSuspend.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderSuspend.Error;
                return View();
            }
            //geriye bir id dönmediğimiz için random orderId atiyoruz.
            return RedirectToAction(nameof(SuccessfulCheckOut), new { orderId = new Random().Next(1,1000) });


            #region Senkron iletişim
            /*
            var orderStatus = await _orderService.CreateOrder(checkOutInfoInput);

            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();
            }
            
            return RedirectToAction(nameof(SuccessfulCheckOut), new { orderId = orderStatus.OrderId });
            */
            #endregion
        }

        public IActionResult SuccessfulCheckOut(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
