using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using NhaSachOnline.Data;
using NhaSachOnline.Models;

namespace NhaSachOnline.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public GenreRepository(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task AddGenre(Genre genre)
        {
            _dbContext.Genres.Add(genre);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteGenre(Genre genre)
        {
            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();
        }

        public Task DeleteGenre(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Genre?> GetGenreById(int id)
        {
            return await _dbContext.Genres.FindAsync(id);
        }

        public async Task<IEnumerable<Genre>> GetGenres()
        {
            return await _dbContext.Genres.ToArrayAsync();
        }

        public async Task UpdateGenre(Genre genre)
        {
            _dbContext.Genres.Update(genre);
            await _dbContext.SaveChangesAsync();
        }
    }
}
