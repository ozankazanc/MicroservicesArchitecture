﻿using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAddress = new Domain.OrderAggragate.Address(
                context.Message.Province,
                context.Message.District,
                context.Message.Street,
                context.Message.ZipCode,
                context.Message.Line
                );

            var order = new Domain.OrderAggragate.Order(context.Message.BuyerId, newAddress);

            foreach (var orderItem in context.Message.OrderItems)
                order.AddOrderItem(orderItem.ProductId, orderItem.ProductName, orderItem.PictureUrl, orderItem.Price);

            await _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
