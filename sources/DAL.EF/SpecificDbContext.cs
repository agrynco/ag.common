using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DAL.EF
{
    public class SpecificDbContext : IdentityDbContext<User>
    {
    }
}