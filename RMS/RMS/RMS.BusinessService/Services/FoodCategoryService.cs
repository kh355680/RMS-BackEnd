using RMS.BusinessModel.Entities;
using RMS.Repository.Repositories;

namespace RMS.BusinessService.Services
{
    public class FoodCategoryService : BaseService<FoodCategory>
    {
        public FoodCategoryService(FoodCategoryRepository repository) : base(repository)
        {

        }
    }
}