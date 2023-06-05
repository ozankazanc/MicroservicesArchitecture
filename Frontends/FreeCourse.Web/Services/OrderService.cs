using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayment;
using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly HttpClient _httpClient;
        private readonly ISharedIdentityService _sharedIdentityService;
        public OrderService(IPaymentService paymentService, IBasketService basketService, HttpClient httpClient, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _basketService = basketService;
            _httpClient = httpClient;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoInput checkOutInfoInput)
        {
            var basket = await _basketService.Get();

            var paymentInfoInput = new PaymentInfoInput
            {
                CardName = checkOutInfoInput.CardName,
                CardNumber = checkOutInfoInput.CardNumber,
                Expiration = checkOutInfoInput.Expiration,
                CVV = checkOutInfoInput.CVV,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);
            if (!responsePayment)
                return new OrderCreatedViewModel { Error = "Ödeme alınamadı.", IsSuccessful = false };

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput
                {
                    District = checkOutInfoInput.Distrinct,
                    Line = checkOutInfoInput.Line,
                    Province = checkOutInfoInput.Province,
                    Street = checkOutInfoInput.Street,
                    ZipCode = checkOutInfoInput.ZipCode
                }
            };

            foreach (var basketItem in basket.BasketItems)
            {
                var orderItemCreateInput = new OrderItemCreateInput
                {
                    ProductId = basketItem.CourseId,
                    PictureUrl = string.Empty,
                    ProductName = basketItem.CourseName,
                    Price = basketItem.GetCurrentPrice
                };
                orderCreateInput.OrderItems.Add(orderItemCreateInput);
            }

            var response = await _httpClient.PostAsJsonAsync("orders", orderCreateInput);
            if (!response.IsSuccessStatusCode)
                return new OrderCreatedViewModel { Error = "Sipariş oluşturalamadı.", IsSuccessful = false };

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            orderCreatedViewModel.Data.IsSuccessful = true;

            await _basketService.Delete();

            return orderCreatedViewModel.Data;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return response.Data;
        }

        public Task SuspendOrder(CheckOutInfoInput checkOutInfoInput)
        {
            throw new NotImplementedException();
        }
    }
}
