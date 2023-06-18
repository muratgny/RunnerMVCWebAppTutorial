using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Data;
using RunnerMVCWebAppTutorial.Interfaces;
using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private AppDbContext _context;
        public RaceRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Add(Race race)
        {
            _context.Races.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race?> GetByIdAsync(int id)
        {
            return await _context.Races.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _context.Races.Where(c => c.Address.City.Contains(city)).Distinct().ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
