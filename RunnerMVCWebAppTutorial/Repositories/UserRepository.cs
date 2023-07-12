using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Data;
using RunnerMVCWebAppTutorial.Interfaces;
using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.Repositories
{
    public class UserRepository : IUserRepository
    {
        AppDbContext _dbContext;
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Add(AppUser user)
        {
            _dbContext.Users.Add(user);
            return Save();
        }

        public bool Delete(AppUser user)
        {
            _dbContext.Remove(user);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public bool Save()
        {
            var saved = _dbContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _dbContext.Update(user);
            return Save();
        }
    }
}
