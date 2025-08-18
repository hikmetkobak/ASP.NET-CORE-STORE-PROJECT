using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class OrderManager : IOrderService
    {
        private readonly IRepositoryManager _manager;
        public OrderManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public IQueryable<Order> Orders => _manager.Order.Orders;

        public int NumberOfInProgress => _manager.Order.NumberOfInProgress;

        public void Complete(int id) => _manager.Order.CompleteOrder(id);
        
        public Order? GetOneOrder(int id) => _manager.Order.GetOneOrder(id);

        public void SaveOrder(Order order) => _manager.Order.SaveOrder(order);
    }
}
