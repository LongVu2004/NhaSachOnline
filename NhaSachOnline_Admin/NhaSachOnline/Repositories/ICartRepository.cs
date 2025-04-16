using NhaSachOnline.Models;
using NhaSachOnline.Models.DTO;

namespace NhaSachOnline.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int bookId, int quantity);
        Task<int> RemoveItem(int bookId);
        Task<Cart> GetUserCart(int id);
        Task<Cart> GetCart(string userId);
        Task<int> GetCartItemCount(string userId = "");
        Task<bool> DoCheckOut(CheckOutModel model);
    }
}
