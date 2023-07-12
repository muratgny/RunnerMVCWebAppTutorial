using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();

        Task<AppUser> GetByIdAsync(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
