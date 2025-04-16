using NhaSachOnline.Models;

namespace NhaSachOnline.Repositories
{
    public interface IGenreRepository
    {
        Task AddGenre(Genre genre);
        Task UpdateGenre(Genre genre);
        Task DeleteGenre(int id);
        Task<Genre?> GetGenreById(int id);
        Task<IEnumerable<Genre>> GetGenres();
    }
}
