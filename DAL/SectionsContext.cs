using Microsoft.EntityFrameworkCore;
using spgnn.Models;

namespace spgnn.DAL
{
    public class SectionsContext : DbContext
    {
        public DbSet<Article> SectionArticles { get; set; }
        
        public SectionsContext(DbContextOptions<SectionsContext> options) : base(options)
        {
        }
    }
}