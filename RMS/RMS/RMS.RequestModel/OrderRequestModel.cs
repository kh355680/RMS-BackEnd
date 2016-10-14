using System.Collections.Generic;

namespace RMS.RequestModel
{
    public class OrderRequestModel
    {
        public List<OrderItemRequestModel> OrderItems { get; set; }
    }
}
