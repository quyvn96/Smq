using Smq.Common.ViewModels;
using Smq.Data.Infrastructure;
using Smq.Data.Repositories;
using Smq.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smq.Service
{
    public interface IOrderService
    {
        bool Create(Order order,List<OrderDetail> orderDetails);
        IEnumerable<Order> GetAll(string keyword);
        bool DeleteOrder(int id);
        IEnumerable<OrderDetail> GetOrderDetailById(int id);
        IEnumerable<OrderDetail> GetAllOrderDetail();
        bool DeleteOrderDetails(int orderId, int productId);
        bool UpdateOrderDetails(int orderId, int productId, int quantity);
        void UpdateStatus(int orderId);
        void Save();
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;
        IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderDetailRepository = orderDetailRepository;
            this._orderRepository = orderRepository;
            this._unitOfWork = unitOfWork;
        }

        public bool Create(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();
                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
              
                return true;
            }catch(Exception ex){
                throw; 
            }
        }
        public void UpdateStatus(int orderId)
        {
            var order = _orderRepository.GetSingleById(orderId);
            order.Status = true;
            _orderRepository.Update(order);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public IEnumerable<Order> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _orderRepository.GetMulti(n => n.CustomerName.Contains(keyword) || n.CustomerMobile.Contains(keyword));
            }
            else
            {
                return _orderRepository.GetAllOrder();
            }
        }
        public bool DeleteOrder(int id)
        {
            return _orderRepository.DeleteOrder(id);
        }
        public IEnumerable<OrderDetail> GetOrderDetailById(int id)
        {
            return _orderDetailRepository.GetOrderDetailById(id);
        }

        public bool DeleteOrderDetails(int orderId, int productId)
        {
            return _orderDetailRepository.DeleteOrderDetails(orderId, productId);
        }

        public bool UpdateOrderDetails(int orderId, int productId, int quantity)
        {
            return _orderDetailRepository.UpdateOrderDetails(orderId, productId, quantity);
        }

        public IEnumerable<OrderDetail> GetAllOrderDetail()
        {
            return _orderDetailRepository.GetAllOrderDetail();
        }
    }
}
