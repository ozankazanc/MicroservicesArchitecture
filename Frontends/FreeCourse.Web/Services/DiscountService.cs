using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Discount;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<DiscountViewModel> Get(string discountCode)
        {
            var response = await _httpClient.GetAsync($"discounts/GetByCode/{discountCode}");
            if (!response.IsSuccessStatusCode)
                return null;

            var discountResult = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();
            return discountResult.Data;
        }
    }
}
