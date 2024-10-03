using OrderApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetOrderByClientId(int clientId);
        Task<OrderDetails> GetOrderDetails(int orderId);
    }
}
