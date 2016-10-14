using RMS.BusinessModel.Context;
using RMS.BusinessModel.Entities;
using RMS.Repository.IRepositories;

namespace RMS.Repository.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(RmsDbContext context) : base(context)
        {
            
        }
    }
}