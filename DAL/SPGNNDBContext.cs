using Microsoft.EntityFrameworkCore;
using spgnn.Models;

namespace spgnn.DAL
{
    public class SpgnndbContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public SpgnndbContext(DbContextOptions<SpgnndbContext> options) : base(options)
        {
        }
    }
}