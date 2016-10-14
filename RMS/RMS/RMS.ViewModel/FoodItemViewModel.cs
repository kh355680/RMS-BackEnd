
namespace RMS.ViewModel
{
    public class FoodItemViewModel
    {
        public string Id { get; set ;}
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
        public FoodCategoryViewModel FoodCategory { get; set; }
    }
}
