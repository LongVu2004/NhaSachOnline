using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NhaSachOnline.Data;
using NhaSachOnline.Models;
using NhaSachOnline.Models.DTO;

namespace NhaSachOnline.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartRepository(ApplicationDbContext dbContext, IHttpContextAccessor iHttpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _contextAccessor = iHttpContextAccessor;
        }

        public Task<int> AddItem(int bookId, int quantity)
        {
            throw new UnauthorizedAccessException("Người dùng chưa đăng nhập");
        }

        public async Task<bool> DoCheckOut(CheckOutModel model)
        {
            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                var userId = GetUserId();

                if(string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("Người dùng chưa đăng nhập");
                }

                var cart = await GetCart(userId);
                if(cart is null)
                {
                    throw new InvalidOperationException("Lỗi, giỏ hàng trống");
                }    

                var orderDetail = _dbContext.CartDetails.Where(c => c.CartId == cart.Id).ToList();
                if(orderDetail.Count == 0)
                {
                    throw new InvalidOperationException("Lỗi, giỏ hàng trống");
                }

                var orderStatus = _dbContext.OrderStatuses.FirstOrDefault(o => o.StatusName == "Đang xử lý");
                if(orderStatus is null)
                {
                    throw new InvalidOperationException("Trạng thái đơn hàng đang chờ xử lý");
                }

                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    isPaid = false,
                    //OrderStatus = orderStatus.StatusId,
                };

                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();

                foreach (var item in orderDetail)
                {
                    var orderDetailItem = new OrderDetail
                    {
                        OrderId = order.Id,
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        UnitPrice = (decimal)item.UnitPrice,
                    };

                    _dbContext.OrderDetails.Add(orderDetailItem);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public async Task<Cart> GetCart(string userId)
        {
            var cart = await _dbContext.Carts.FirstOrDefaultAsync(u => u.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if(string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }   
            var data = await (
                from cart in _dbContext.Carts
                join cartDetail in _dbContext.CartDetails
                on cart.Id equals cartDetail.CartId
                where cart.UserId == userId
                select new {cartDetail.Id} 
                ).ToListAsync();
            return data.Count();
        }

        public async Task<Cart> GetUserCart(int id)
        {
            var userId = GetUserId();
            if(userId == null)
            {
                throw new InvalidOperationException("Không thể tìm thấy id");
            }   
            var cart = await _dbContext.Carts
                .Include(cart => cart.CartDetails)
                .ThenInclude(b => b.Book)
                .ThenInclude(stock => stock.Stock)
                .Include(cartDetail => cartDetail.CartDetails)
                .ThenInclude(b => b.Book)
                .ThenInclude(b => b.Genre)
                .Where(userid => userid.UserId == userId).FirstOrDefaultAsync();

            return cart;
        }

        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            // xử lý ngoại lệ
            try
            {
                if(string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("Bạn chưa đăng nhập");
                }
                
                var cart = await GetCart(userId);
                if(cart is null)
                {
                    throw new UnauthorizedAccessException("Giỏ hàng trống");
                }
                
                var cartItem = _dbContext.CartDetails
                    .FirstOrDefault(s => s.CartId == cart.Id && s.BookId == bookId);

                if (cartItem is null)
                {
                    throw new InvalidOperationException("Không có sản phẩm nào trong giỏ hàng");
                }
                else if(cartItem.Quantity == 1)
                {
                    _dbContext.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity--;
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException("Lỗi, vui lòng chạy lại");
            }

            var cartItemCount = await GetCartItemCount(userId);

            return cartItemCount;
        }

        private string GetUserId()
        {
            //var nhanDienNguoiDung = _contextAccessor.HttpContext.User;
            //string userId = _userManager.GetUserId(nhanDienNguoiDung);
            //return userId;

            var httpContext = _contextAccessor.HttpContext;

            if(httpContext?.User != null)
            {
                return _userManager.GetUserId(httpContext.User);
            }
            return null;
        }
    }
}
