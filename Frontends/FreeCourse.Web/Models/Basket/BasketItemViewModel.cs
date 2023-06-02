using FreeCourse.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models.Basket
{
    public class BasketItemViewModel
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } = 1;
        private decimal? DiscountAppliedPrice { get; set; }
        public decimal GetCurrentPrice { get { return DiscountAppliedPrice.IsNotNull() ? DiscountAppliedPrice.Value : Price; } }
        public void AppliedDiscount(decimal disCountPrice)
        {
            DiscountAppliedPrice = disCountPrice;
        }
    }
}
