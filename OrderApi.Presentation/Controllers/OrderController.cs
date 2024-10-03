using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Application.DTO;
using OrderApi.Application.DTO.Conversion;
using OrderApi.Application.Interface;
using OrderApi.Application.Services;

namespace OrderApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any())
                return NotFound("orders not foundin database");
            var (_,list) = OrderConversion.FromEntity(null, orders);
            return !list!.Any()?  NotFound() : Ok(list);

        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await orderInterface.GetByIdAsync(id);
            if (order == null)
                return NotFound(null);

            var (_order, _) = OrderConversion.FromEntity(order, null);
            return Ok(_order);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrder(int clientId)
        {
            if (clientId == 0) return BadRequest("invalid data provided");
            var orders = await orderService.GetOrderByClientId(clientId);
            return !orders.Any() ? NotFound(null) : Ok(orders);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid data Provided");
            var orderDetails = await orderService.GetOrderDetails(orderId);
            return orderDetails.OrderId > 0 ? Ok(orderDetails) : NotFound("Order not Found");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incomplete data subbmited");

            var getentity = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(getentity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> deleteOrder(OrderDTO orderDTO)
        {
            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }


    }
}
