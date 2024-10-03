using OrderApi.Application.DTO;
using OrderApi.Application.DTO.Conversion;
using OrderApi.Application.Interface;
using Polly;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface ,HttpClient httpClient,
        ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        // Get product 

        public async Task<ProductDTO> GetProduct(int productId)
        {
            //call product api using httpclient
            //Redirect thiscall to the Api Gateway since product api  is not response to outsiders.

            var getProduct = await httpClient.GetAsync($"/api/Product/{productId}");
            if(!getProduct.IsSuccessStatusCode)
            {
                return null!;
            }
            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        //get user 

        public async Task<AppUserDTO> GetUser(int userId)
        {
            //call product api using httpclient
            //Redirect thiscall to the Api Gateway since product api  is not response to outsiders.

            var getUser = await httpClient.GetAsync($"/api/Product/{userId}");
            if (!getUser.IsSuccessStatusCode)
            {
                return null!;
            }
            var product = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return product!;
        }

        //get order by clientid
        public async Task<IEnumerable<OrderDTO>> GetOrderByClientId(int clientId)
        {
         //get all client order
          var orders = await orderInterface.GetOrdersAsync(p => p.ClientId == clientId);
            if (!orders.Any()) return null!;

            //convert from entity to dto 
            var (_,_orders) = OrderConversion.FromEntity(null,orders);
            return _orders!;



        }

        //get order deatails
        public async Task<OrderDetails> GetOrderDetails(int orderId)
        {
            // prepare order 

            var order = await orderInterface.GetByIdAsync(orderId);
            if (order is null || order!.Id <= 0)
                return null!;

            //get retry pipeline
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            // prepare product 
            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            //Prepare client
            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            //populate order deatails 
            return new OrderDetails(
                order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchaseQuantity,
                order.OrderedDate  //may get error in name 
                );

        }
    }
}
