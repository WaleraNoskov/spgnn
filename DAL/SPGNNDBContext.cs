using Microsoft.EntityFrameworkCore;
using spgnn.Models;

namespace spgnn.DAL
{
    public class SPGNNDBContext : DbContext
    {
        DbSet<Article> Articles { get; set; }
        DbSet<FileModel> Files { get; set; }
        public SPGNNDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}