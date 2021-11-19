using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestShopApplication.Dal.Common;

namespace TestShopApplication.Dal.Repositories
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetAll(Guid userId);
    }
}