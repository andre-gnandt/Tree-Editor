using Microsoft.EntityFrameworkCore;
using LocalTreeData.Models;

namespace LocalTreeData.EfCore
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
        : base(options)
        {
        }

        public DbSet<Models.File> Files { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Tree> Trees { get; set; }
        public DbSet<ConfigType> ConfigTypes { get; set; }
    }
}

