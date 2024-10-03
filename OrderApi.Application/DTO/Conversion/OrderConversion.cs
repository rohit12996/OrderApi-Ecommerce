using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTO.Conversion
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO order) => new Order()
        {
            Id = order.Id,
            ClientId = order.ClientId,
            ProductId = order.ProductId,
            OrderedDate = order.OrderedDate,
            PurchaseQuantity = order.PurchaseQuantity
        };

        public static (OrderDTO? , IEnumerable<OrderDTO>?) FromEntity(Order?  order, IEnumerable<Order>? orders)
        {
            // return single
            if (orders == null || order is not null)
            {
                var singleOrder = new OrderDTO(
                    order!.Id,
                    order.ClientId,
                    order.ProductId,
                    order.PurchaseQuantity,
                    order.OrderedDate);

                    return (singleOrder, null);
            }

            //return list 

            if(orders != null || order is  null)
            {
                var _order = orders!.Select(p =>
                new OrderDTO(
                    p.Id,
                    p.ClientId,
                    p.ProductId,
                    p.PurchaseQuantity,
                    p.OrderedDate
                ));
                return (null, _order);
            }

            return (null, null);
        }
    }
}
