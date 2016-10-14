using System;
using System.Collections.Generic;

namespace RMS.ViewModel
{
    public class OrderViewModel
    {
        public string OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Total { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
