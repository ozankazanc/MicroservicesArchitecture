﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Dtos
{
    public class OrderDto
    {
        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        public AddressDto Address { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
