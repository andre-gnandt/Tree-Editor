using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LocalTreeData.Models
{
    public class Node
    {
        private ICollection<Node> _children;
        private ILazyLoader LazyLoader { get; set; }
        private static bool LoadChildren;

        public Node() { }
        private Node(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public Guid Id { get; set; }
        public Guid? NodeId { get; set;}
        public string? Data { get; set; }
        public ICollection<Node> Children 
        {
            get => LoadChildren ? LazyLoader.Load(this, ref _children) : new List<Node>();
            set => _children = value;
        }
        public Node? Parent { get; set; }
        public int? Level { get; set; }
        public int? Number {  get; set; }
        public string? Title { get; set; }

        public static void LoadEntities(bool load) { LoadChildren = load; }
    }
}
