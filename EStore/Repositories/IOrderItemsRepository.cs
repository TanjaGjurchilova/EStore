using EStore.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EStore.Repositories
{
    interface IOrderItemsRepository
    {
        OrderItems FindOrderItemById(long id);
        IEnumerable<OrderItems> FindOrderItemByOrderId(long orderId);
        IEnumerable<OrderItems> GetAllOrderItems();
        void SaveOrderItem(OrderItems orderItem);
        void UpdateOrderItem(OrderItems orderItem);
        void DeleteOrderItem(OrderItems orderItem);
    }
}
