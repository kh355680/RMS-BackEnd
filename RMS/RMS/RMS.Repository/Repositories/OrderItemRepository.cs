using RMS.BusinessModel.Context;
using RMS.BusinessModel.Entities;

namespace RMS.Repository.Repositories
{
    public class OrderItemRepository : BaseRepository<OrderItem>
    {
        public OrderItemRepository(RmsDbContext context) : base(context)
        {
            
        }
    }
}