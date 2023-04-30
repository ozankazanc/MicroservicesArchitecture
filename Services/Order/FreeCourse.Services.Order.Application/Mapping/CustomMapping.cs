using AutoMapper;
using FreeCourse.Services.Order.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Mapping
{
    class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<Order.Domain.OrderAggragate.Order, OrderDto>().ReverseMap();
            CreateMap<Order.Domain.OrderAggragate.OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Order.Domain.OrderAggragate.Address, AddressDto>().ReverseMap();
        }
    }
}
