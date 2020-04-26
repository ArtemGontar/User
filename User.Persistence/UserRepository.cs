using Shared.Identity;
using Shared.Persistence.MySql;

namespace User.Persistence
{
    public class UserRepository : SqlRepository<ApplicationUser>
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
