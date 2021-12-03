using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Models;

namespace TestShopApplication.Dal.Repositories
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAll(Guid userId);
    }
}