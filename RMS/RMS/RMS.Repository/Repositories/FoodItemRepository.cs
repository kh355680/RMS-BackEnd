using RMS.BusinessModel.Context;
using RMS.BusinessModel.Entities;
using RMS.Repository.IRepositories;

namespace RMS.Repository.Repositories
{
    public class FoodItemRepository : BaseRepository<FoodItem>, IFoodItemRepository
    {
        public FoodItemRepository(RmsDbContext context) : base(context)
        {
            
        }
 
    }
}