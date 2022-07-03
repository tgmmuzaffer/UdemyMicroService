using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumer
{
    public class CoursNameChangedNameEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _orderDbContext;

        public CoursNameChangedNameEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItems = await _orderDbContext.OrderItems.Where(a => a.ProductId == context.Message.CourseId).ToListAsync();
            orderItems.ForEach(z =>
            {
                z.UpdateOrderItem(context.Message.UpdatedName, z.PictureUrl, z.Price);
            });

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
