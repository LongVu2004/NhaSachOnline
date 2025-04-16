using NhaSachOnline.Models;

namespace NhaSachOnline.Repositories
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> GetBookInfoFromDatabase(string keySearch = "", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}
