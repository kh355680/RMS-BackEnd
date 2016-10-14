using RMS.BusinessModel.Context;
using RMS.BusinessModel.Entities;
using RMS.Repository.IRepositories;

namespace RMS.Repository.Repositories
{
    public class FoodCategoryRepository : BaseRepository<FoodCategory>, IFoodCategoryRepository
    {
        public FoodCategoryRepository(RmsDbContext context) : base(context)
        {
        }
    }
}
