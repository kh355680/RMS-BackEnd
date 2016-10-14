using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.BusinessModel.Entities
{
    public class OrderItem : Entity
    {
        public string OrderId { get; set; }
        public string FoodItemId { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }

        // Navigation Properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("FoodItemId")]
        public virtual FoodItem FoodItem { get; set; }

    }
}