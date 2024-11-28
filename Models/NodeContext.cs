using Microsoft.EntityFrameworkCore;

namespace LocalTreeData.Models
{
    public class NodeContext : DbContext
    {
        public NodeContext(DbContextOptions<NodeContext> options)
        : base(options)
        {
            //this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<File> Files { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Tree> Trees { get; set; }
    }
}
