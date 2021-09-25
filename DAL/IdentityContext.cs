using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using spgnn.Models;

namespace spgnn.DAL
{
    public class IdentityContext : IdentityDbContext<User>
    {
        
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
    }
}