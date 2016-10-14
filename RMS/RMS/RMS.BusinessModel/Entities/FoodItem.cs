using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.BusinessModel.Entities
{
    public class FoodItem : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
        public string FoodCategoryId { get; set; }

        // Navigation Properties
        [ForeignKey("FoodCategoryId")]
        public FoodCategory FoodCategory { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } 
    }
}
