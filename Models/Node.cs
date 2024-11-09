using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LocalTreeData.Models
{
    public class Node
    {
        private ICollection<Node> _children;
        private ICollection<File> _files;
        private ILazyLoader LazyLoader { get; set; }
        private static bool loadChildren;
        private static bool loadFiles;

        public Node() { }
        private Node(ILazyLoader lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public Guid Id { get; set; }
        public Guid? NodeId { get; set;}
        public string? Data { get; set; }
        public string? ThumbnailId { get; set; }
        public ICollection <Node> Children 
        {
            get => loadChildren ? LazyLoader.Load(this, ref _children).Where(q => !q.IsDeleted).ToList() : new List<Node>();
            set =>  _children = value;
        }
        public ICollection<File> Files
        {
            get => loadFiles && ThumbnailId != null ? LazyLoader.Load(this, ref _files).Where(q => q.Id == new Guid(this.ThumbnailId) && !q.IsDeleted).ToList() : new List<File>();
            set => _files = value;
        }
        public Node? Parent { get; set; }
        public int? Level { get; set; }
        public int? Number {  get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? RankId { get; set; }
        public bool IsDeleted { get; set; }
       

        public static void LoadChildren(bool load) { loadChildren = load; }
        public static void LoadFiles(bool load) { loadFiles = load; }
    }
}
