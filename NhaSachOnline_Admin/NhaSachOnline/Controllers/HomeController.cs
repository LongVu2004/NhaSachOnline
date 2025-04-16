using Microsoft.AspNetCore.Mvc;
using NhaSachOnline.Models;
using NhaSachOnline.Models.DTO;
using NhaSachOnline.Repositories;
using System.Diagnostics;

namespace NhaSachOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string keySearch = "", int genreId = 0)
        {
            IEnumerable<Book> books = await _homeRepository.GetBookInfoFromDatabase(keySearch, genreId);
            IEnumerable<Genre> genres = await _homeRepository.Genres();
            BookDislayModel bookDislayModel = new BookDislayModel()
            {
                Books = books.Select(b => new BookDTO
                {
                    Id = b.Id,
                    BookName = b.BookName,
                    ImagePath = b.Image, // Updated to match the new property name
                    AuthorName = b.AuthorName,
                    Price = b.Price,
                    Quantity = b.Quantity,
                    GenreName = genres.FirstOrDefault(g => g.Id == b.GenreId)?.GenreName ?? "Không rõ"
                }).ToList(),
                Genres = genres.Select(g => new GenreDTO
                {
                    Id = g.Id,
                    GenreName = g.GenreName
                }).ToList(),
                KeySearch = keySearch,
                GenreId = genreId
            };

            // Return the view with the updated model
            return View(bookDislayModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}