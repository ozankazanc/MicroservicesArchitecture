using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages;
using FreeCourse.Shared.Services;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Consumer
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly RedisService _redisService;
        public CourseNameChangedEventConsumer(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var existBasket = await _redisService.GetDb().StringGetAsync(context.Message.UserId);
            var basketDto = JsonSerializer.Deserialize<BasketDto>(existBasket);

            foreach (var basketItem in basketDto.BasketItems.Where(x => x.CourseId == context.Message.CourseId))
            {
                basketItem.CourseName = context.Message.UpdatedName;
            }

            await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));
        }
    }
}
