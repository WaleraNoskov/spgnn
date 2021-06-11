using Microsoft.EntityFrameworkCore;
using spgnn.Models;

namespace spgnn.DAL
{
    public class InfoArticlesContext : DbContext
    {
        public DbSet<Article> InfoArticles { get; set; }
        public InfoArticlesContext(DbContextOptions<InfoArticlesContext> options) : base(options)
        {
        }
    }
}