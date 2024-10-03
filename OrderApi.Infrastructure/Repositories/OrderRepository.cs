using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interface;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var  order = context.Orders.Add(entity).Entity;
                await  context.SaveChangesAsync();
                return order.Id > 0 ? new Response(true, "Order Placed successfully") :
                    new Response(false, "Error occured while placing Order");
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                return new Response(false, "Error occoured while placing order");
            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.Id);
                if (order is null)
                    return new Response(false, "order not found");
                context.Orders.Remove(entity);//entity  --use it 
                await context.SaveChangesAsync();
                return new Response(true, "Order successfully deleated");
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                return new Response(false, "Error occoured while deleting order");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var order = await context.Orders.AsNoTracking().ToListAsync();
                return order is not null ? order : null!;
                
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                throw new Exception("Error occoured while retriving  order");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {

            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                throw new Exception("Error occoured while getting  order");
            }
           
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                   var order = await context.Orders.FindAsync(id);
                if (order is not null) return order;
                else return null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception( "Error occured while retriving order");

            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).ToListAsync();
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                throw new Exception("Error occoured while getting  order");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.Id);
                if (order is null) return new Response(false, $"order not found while updating ");
                
                context.Entry(order).State = EntityState.Detached;
                await context.SaveChangesAsync();
                return new Response(true,"order updated successfully");

            }
            catch (Exception ex)
            {

                LogException.LogExceptions(ex);

                return new Response(false, "Error occoured while placing order");
            }
        }
    }
}
