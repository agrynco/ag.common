using DAL.Abstract;
using DAL.EF.Core;
using Domain;

namespace DAL.EF
{
    public class UsersRepository : BaseRepository<SpecificDbContext, User, long>, IUsersRepository
    {
        public UsersRepository(SpecificDbContext dbContext) : base(dbContext)
        {
        }
    }
}