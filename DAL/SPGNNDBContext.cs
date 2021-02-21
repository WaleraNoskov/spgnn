using Microsoft.EntityFrameworkCore;
using spgnn.Models;

namespace spgnn.DAL
{
    public class SpgnndbContext : DbContext
    {
        DbSet<Article> Articles { get; set; }
        public SpgnndbContext(DbContextOptions options) : base(options)
        {
        }
    }
}