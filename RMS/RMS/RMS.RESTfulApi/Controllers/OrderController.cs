using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using RMS.BusinessModel.Context;
using RMS.BusinessService.Services;
using RMS.Repository.Repositories;
using RMS.RequestModel;
using RMS.ViewModel;
using ResponseModel = RMS.RequestModel.ResponseModel;

namespace RMS.RESTfulApi.Controllers
{
    [RoutePrefix("api/order")]
    public class OrderController : ApiController
    {
        private readonly OrderService _orderService;
        private readonly OrderItemService _orderItemService;

        public OrderController()
        {
            _orderService = new OrderService(new OrderRepository(new RmsDbContext()));
            _orderItemService = new OrderItemService(new OrderItemRepository(new RmsDbContext()));
        }

        [Route("save")]
        [HttpPost]
        public async Task<IHttpActionResult> MakeOrder(OrderRequestModel orderRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = _orderService.SaveNewOrder(orderRequest);

            if (response == null)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Order Saved Success",
                Data = response
            });
        }

        [HttpGet]
        [Route("detail/{id}")]
        public IHttpActionResult GetOrderDetail(string id)
        {
            var response = _orderService.GetOrderDetailById(id);

            if (response == null)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Order Detail",
                Data = response
            });
        }

        [HttpPost]
        [Route("report")]
        public IHttpActionResult GetOrdersByDate([FromBody]OrderReportRequestModel order)
        {
            var orders = _orderService.Get().Where(x => x.OrderDate == order.OrderDate).ToList();

            var ordersViewModel = new List<OrderViewModel>();

            foreach (var orderTemp in orders)
            {
                ordersViewModel.Add(new OrderViewModel
                {
                    OrderId = orderTemp.Id,
                    Total = Convert.ToInt32(orderTemp.Total)
                });
            }

            return Ok(ordersViewModel);
        }

        [Route("delete/{orderId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteOrder([FromUri]string orderId)
        {
            var order = _orderService.Get(orderId);

            var orderItems = await _orderItemService.Get().Where(x => x.OrderId == orderId).ToListAsync();

            orderItems.ForEach(x => _orderItemService.Remove(x.Id));

            var response = _orderService.Remove(order.Id);

            if (response == null)
                return InternalServerError();

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Order Deleted"
            });
        }
    }
}
