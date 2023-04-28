using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.CotrollerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountService.GetById(id);
            return CreateActionResultInstance(discount);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;

            var discount = await _discountService.GetByCodeAndUserId(code, userId);
            return CreateActionResultInstance(discount);
        }
        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount)
        {
            var res = await _discountService.Save(discount);
            return CreateActionResultInstance(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount)
        {
            var res = await _discountService.Update(discount);
            return CreateActionResultInstance(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _discountService.Delete(id);
            return CreateActionResultInstance(res);
        }
    }
}
