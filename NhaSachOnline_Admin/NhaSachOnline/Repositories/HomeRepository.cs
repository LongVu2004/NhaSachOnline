using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NhaSachOnline.Data;
using NhaSachOnline.Models;

namespace NhaSachOnline.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _dbContext;


        public HomeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _dbContext.Genres.ToListAsync();
        }

        // lấy thông tin sách từ csdl
        public async Task<IEnumerable<Book>> GetBookInfoFromDatabase(string keySearch = "", int genreId = 0)
        {
            // chuyển dổi chuỗi keySearch sang dạng chữ thường
            keySearch = keySearch.ToLower();

            IEnumerable<Book> books = await (from book in _dbContext.Books
                         join genre in _dbContext.Genres
                         on book.GenreId equals genre.Id 
                         where string.IsNullOrWhiteSpace(keySearch) || 
                                        (book != null && book.BookName.ToLower().StartsWith(keySearch.ToLower()))
                         select new Book
                         {
                             Id = book.Id,
                             BookName = book.BookName,
                             Image = book.Image,
                             AuthorName = book.AuthorName,
                             Description = book.Description,
                             Price = book.Price,
                             GenreId = book.GenreId,
                             GenreName = book.GenreName,
                             Quantity = book.Quantity,
                         }
                         ).ToListAsync();

            if(genreId > 0)
            {
                books = books.Where(g => g.GenreId == genreId).ToList();
            }    

            return books;
        }
    }
}
