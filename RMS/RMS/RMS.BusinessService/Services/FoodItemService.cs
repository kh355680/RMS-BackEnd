using RMS.BusinessModel.Entities;
using RMS.Repository.Repositories;

namespace RMS.BusinessService.Services
{
    public class FoodItemService : BaseService<FoodItem>
    {
        public FoodItemService(FoodItemRepository repository) : base(repository)
        {
           
        }
    }
}