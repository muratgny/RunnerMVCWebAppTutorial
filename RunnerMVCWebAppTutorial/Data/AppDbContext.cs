using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunnerMVCWebAppTutorial.Models;

namespace RunnerMVCWebAppTutorial.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>//user authentication ve user rolleri için identitydbcontext e cevirdik
        //normalde dbcontext i <> hariç kullanıyorduk. Eger user rolleri için ayrı bir class ımız varsa AppUser ın yanına ekleyebiliriz
        //AppUser clasımız hazır olduktan ve burayı identity e göre düzenledikten sonra migration yapıp db leri düzenledik
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        } 

        public DbSet<Race> Races { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Address> Addresses { get; set; }
        //public DbSet<Race> Races { get; set; }
        //public DbSet<Race> Races { get; set; }


    }
}
