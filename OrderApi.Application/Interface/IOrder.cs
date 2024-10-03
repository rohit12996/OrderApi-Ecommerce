﻿using eCommerce.SharedLibrary.Interface;
using OrderApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.Interface
{
    public interface IOrder : IGenericInterface<Order> 
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
    }
}
