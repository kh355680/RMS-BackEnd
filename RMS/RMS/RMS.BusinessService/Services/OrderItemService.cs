using RMS.BusinessModel.Entities;
using RMS.Repository.Repositories;

namespace RMS.BusinessService.Services
{
    public class OrderItemService : BaseService<OrderItem>
    {
        public OrderItemService(OrderItemRepository repository) : base(repository)
        {
            
        }
    }
}