using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggragate
{
    /// <summary>
    /// DOMAIN KATMANI HERHANGI BIR ORM ARACINA BAGLI-BAGIMLI OLMAMALIDIR. BUNLARI INFRASTRUCTURE KATMANINDA COZECEGIZ.
    /// </summary>
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }

        /// <summary>
        /// backing field
        /// </summary>
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {

        }
        public Order(string buyerId, Address address)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        public void AddOrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);
            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItems.Add(newOrderItem);
            }
        }
        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);

        /// <summary>
        /// EFCore'u burada iki şekilde hareket ettirebiliriz:
        /// 1. Address'in içerisinde property'leri direk Order tablosunda alan olarak geçirmesini sağlattırabiliriz.
        /// 2. Address adında bir tablo oluşturtarak, ilişkilendirme yaptırabiliriz.
        /// Owned Entity Types adıyla geçmektedir. (Attribute ile sağlanmakta [Owned])
        /// </summary>
        public Address Address { get; set; }

    }
}
