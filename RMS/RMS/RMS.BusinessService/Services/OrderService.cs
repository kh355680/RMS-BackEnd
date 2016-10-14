using System;
using System.Collections.Generic;
using System.Linq;
using RMS.BusinessModel.Context;
using RMS.BusinessModel.Entities;
using RMS.Repository.Repositories;
using RMS.RequestModel;
using RMS.ViewModel;

namespace RMS.BusinessService.Services
{
    public class OrderService : BaseService<Order>
    {

        private readonly OrderItemService _orderItemService;
        public OrderService(OrderRepository repository):base(repository)
        {
           
            _orderItemService = new OrderItemService(new OrderItemRepository(new RmsDbContext()));            
            
            //_orderService = new OrderService(new OrderRepository(RmsDbContext.GetDbContext()));
            //_orderItemService = new OrderItemService(new OrderItemRepository(RmsDbContext.GetDbContext()));        
        }

        public OrderViewModel SaveNewOrder(OrderRequestModel orderRequestModel)
        {
            var order = new Order();
            var foodItems = new List<FoodItem>();
            var foodItemRepository = new FoodItemService(new FoodItemRepository(new RmsDbContext()));

            foreach (var orderItemRequestModel in orderRequestModel.OrderItems)
            {
                foodItems.Add(foodItemRepository.Get(orderItemRequestModel.FoodItemId));
            }

            
            order.Total = CalculateOrderTotal(orderRequestModel.OrderItems, foodItems);
            order.OrderDate = DateTime.Today;

            var savedOrder = Insert(order);

            var orderItemResponseList = new List<OrderItemViewModel>();

            
            for (int i = 0; i < orderRequestModel.OrderItems.Count; i++)
            {
                var orderItem = new OrderItem
                {
                    OrderId = savedOrder.Id,
                    FoodItemId = foodItems[i].Id,
                    Quantity = orderRequestModel.OrderItems[i].Quantity,
                    Total = orderRequestModel.OrderItems[i].Quantity*foodItems[i].Rate
                };

                var savedOrderItem = _orderItemService.Insert(orderItem);

                orderItemResponseList.Add(new OrderItemViewModel
                {
                    OrderItemId = savedOrderItem.OrderId,
                    FoodItemId = savedOrderItem.FoodItemId,
                    Quantity = savedOrderItem.Quantity,
                    Cost = savedOrderItem.Total
                });
            }
            
            var responseModel = new OrderViewModel
            {
                OrderId = order.Id,
                Total = Convert.ToInt32(order.Total),
                OrderDate = order.OrderDate,
                OrderItems = orderItemResponseList
            };

           return responseModel;
        }

        private int CalculateOrderTotal(List<OrderItemRequestModel> orderItems,List<FoodItem> selectedFoodItems)
        {
            var total = 0;

            for (int i = 0; i < orderItems.Count; i++)
            {
                total += orderItems[i].Quantity*selectedFoodItems[i].Rate;
            }

            return total;
        }


        public OrderViewModel GetOrderDetailById(string id)
        {
            var order = Get(id);

            var orderItems = _orderItemService.Get().Where(x => x.OrderId == id).ToList();

            var orderItemVm = new List<OrderItemViewModel>();

            foreach (var orderItem in orderItems)
            {
                orderItemVm.Add(new OrderItemViewModel
                {
                    FoodItemId = orderItem.FoodItemId,
                    OrderItemId = orderItem.OrderId,
                    Quantity = orderItem.Quantity,
                    Cost = orderItem.Total
                });
            }

            return new OrderViewModel
            {
                
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                Total = Convert.ToInt32(order.Total),
                OrderItems = orderItemVm
            };            
        }
    }
}