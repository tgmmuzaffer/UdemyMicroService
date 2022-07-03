using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages;
using MassTransit;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Consumers
{
    public class BasketCourseNameChangedEventConsumer : IConsumer<BasketCourseNameChangedEvent>
    {
        private readonly RedisService _redisService;

        public BasketCourseNameChangedEventConsumer(RedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task Consume(ConsumeContext<BasketCourseNameChangedEvent> context)
        {
            var existbasket = await _redisService.GetDb().StringGetAsync(context.Message.UserId);
            if (!string.IsNullOrEmpty(existbasket))
            {
               var redis_data  = JsonSerializer.Deserialize<BasketDto>(existbasket);
                foreach (var item in redis_data.basketItems.Where(a=>a.CourseId==context.Message.CourseId))
                {
                    item.CourseName = context.Message.UpdatedName;
                }

                await _redisService.GetDb().StringSetAsync(context.Message.UserId, JsonSerializer.Serialize(redis_data));
            }


        }
    }
}
