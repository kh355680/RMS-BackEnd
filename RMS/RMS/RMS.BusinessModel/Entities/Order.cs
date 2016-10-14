using System;
using System.Collections.Generic;

namespace RMS.BusinessModel.Entities
{
    public class Order : Entity
    {
        public DateTime OrderDate { get; set; }
        public double Total { get; set; }
        
        public virtual ICollection<OrderItem> OrderItems { get; set; }         
    }
}