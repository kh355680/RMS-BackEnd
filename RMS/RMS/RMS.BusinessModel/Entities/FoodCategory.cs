using System.Collections.Generic;

namespace RMS.BusinessModel.Entities
{
    public class FoodCategory : Entity
    {
        public string Name { get; set; }
        // Navigation Properties
        public virtual ICollection<FoodItem> FoodItems { get; set; } 
    }
}
