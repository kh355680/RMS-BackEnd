using RMS.BusinessModel.Entities;
using RMS.ViewModel;

namespace RMS.UtilityService
{
    public static class FoodCategoryEntityToVmConverter
    {
        public static FoodCategoryViewModel Convertion(FoodCategory foodCategory)
        {
            return new FoodCategoryViewModel
            {
                Id = foodCategory.Id,
                Name = foodCategory.Name
            };
        }
    }
}
