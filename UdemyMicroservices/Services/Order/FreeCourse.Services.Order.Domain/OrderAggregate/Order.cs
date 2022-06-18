using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get;private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private set; }
        private readonly List<OrderItem> _orderItem;
        public IReadOnlyCollection<OrderItem> OrderItems=> _orderItem;
        public Order()
        {

        }
        public Order(string buyerid, Address address)
        {
            _orderItem = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId= buyerid;
            Address= address;
        }

        public void AddOrderItem(int productId, string productName, decimal price, string pictureUrl)
        {
            var existProduct = _orderItem.Any(x => x.ProductId == productId);
            if (!existProduct)
            {
                var newOrdederItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItem.Add(newOrdederItem);
            }
        }

        public decimal GetTotalPrice => _orderItem.Sum(x => x.Price);

    }
}
