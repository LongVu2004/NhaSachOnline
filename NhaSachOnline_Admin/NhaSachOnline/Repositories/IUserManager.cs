using NhaSachOnline.Models;

namespace NhaSachOnline.Repositories
{
    public interface IUserManager
    {
        Task<IEnumerable<Order>> UserOrders(bool getAll = false);
        Task<Order?> GetOrderById(int id);
        Task<IEnumerable<OrderStatus>> GetOrderStatus();
        Task TogglePaymentMethod(int orderId);
        //Task ChangeOrderStatus(int orderId, int statusId);
    }
}
