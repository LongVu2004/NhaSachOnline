using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaSachOnline.Data;
using NhaSachOnline.Models;
using NhaSachOnline.Repositories;

namespace NhaSachOnline.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartRepository _cartRepository;
        private readonly ApplicationDbContext _context;

        public CartController(ILogger<CartController> logger, ICartRepository cartRepository, ApplicationDbContext context)
        {
            _logger = logger;
            _cartRepository = cartRepository;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.Identity.Name; // Hoặc lấy từ Session/Login user
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                    .ThenInclude(cd => cd.Book)
                .FirstOrDefault(c => c.UserId == userId && !c.IsDeleted);

            if (cart == null)
            {
                return View(new List<CartItem>());
            }

            var cartItems = cart.CartDetails.Select(cd => new CartItem
            {
                BookId = cd.BookId,
                BookTitle = cd.Book.BookName,
                //ImageUrl = cd.Book.Image,
                Price = (decimal)cd.UnitPrice,
                Quantity = cd.Quantity
            }).ToList();

            return View(cartItems);

            //var userId = User.Identity.Name;

            //var cart = _context.Carts
            //    .Include(c => c.CartDetails)
            //    .ThenInclude(cd => cd.Book)
            //    .FirstOrDefault(c => c.UserId == userId && !c.IsDeleted);

            //return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int bookId)
        {
            var userId = User.Identity.Name;
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefault(c => c.UserId == userId && !c.IsDeleted);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    IsDeleted = false,
                    CartDetails = new List<CartDetail>()
                };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var cartItem = cart.CartDetails.FirstOrDefault(cd => cd.BookId == bookId);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
            }
            else
            {
                //var book = _context.Books.Find(bookId);
                //cart.CartDetails.Add(new CartDetail
                //{
                //    BookId = bookId,
                //    Quantity = 1,
                //    UnitPrice = book.Price,
                //});
                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    return Json(new { success = false, message = "Book not found." });
                }

                cart.CartDetails.Add(new CartDetail
                {
                    BookId = bookId,
                    Quantity = 1,
                    UnitPrice = book.Price
                });
            }

            _context.SaveChanges();

            // Cập nhật session hoặc temp data
            var cartCount = cart.CartDetails.Sum(cd => cd.Quantity);
            HttpContext.Session.SetInt32("CartCount", cartCount);

            return Json(new { success = true, cartCount });
        }

        [HttpPost]
        public IActionResult IncreaseQuantity(int bookId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.BookId == bookId);

            if (item != null)
            {
                item.Quantity++;
                SaveCart(cart);
                var cartCount = cart.Sum(x => x.Quantity);
                HttpContext.Session.SetInt32("CartCount", cartCount);

                return Json(new
                {
                    success = true,
                    quantity = item.Quantity,
                    total = item.Quantity * item.Price,
                    cartCount
                });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DecreaseQuantity(int bookId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.BookId == bookId);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }
                SaveCart(cart);
                return Json(new
                {
                    success = true,
                    quantity = item.Quantity,
                    total = item.Quantity * item.Price,
                    cartCount = cart.Sum(x => x.Quantity)
                });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int bookId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.BookId == bookId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
                return Json(new
                {
                    success = true,
                    cartCount = cart.Sum(x => x.Quantity)
                });
            }
            return Json(new { success = false });
        }
        private List<CartItem> GetCart()
        {
            var userId = User.Identity.Name;
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                .ThenInclude(cd => cd.Book)
                .FirstOrDefault(c => c.UserId == userId && !c.IsDeleted);

            if (cart == null)
            {
                return new List<CartItem>();
            }

            return cart.CartDetails.Select(cd => new CartItem
            {
                BookId = cd.BookId,
                BookTitle = cd.Book.BookName,
                Price = (decimal)cd.UnitPrice,
                Quantity = cd.Quantity
            }).ToList();
        }

        private void SaveCart(List<CartItem> cartItems)
        {
            var userId = User.Identity.Name;
            var cart = _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefault(c => c.UserId == userId && !c.IsDeleted);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    IsDeleted = false,
                    CartDetails = new List<CartDetail>()
                };
                _context.Carts.Add(cart);
            }

            cart.CartDetails.Clear();
            foreach (var item in cartItems)
            {
                var book = _context.Books.Find(item.BookId);
                if (book != null)
                {
                    cart.CartDetails.Add(new CartDetail
                    {
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        UnitPrice = book.Price
                    });
                }
            }

            _context.SaveChanges();
        }
    }
}
