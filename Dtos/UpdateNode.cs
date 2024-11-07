using LocalTreeData.Models;

namespace LocalTreeData.Dtos
{
    public class UpdateNode
    {
        public Guid Id { get; set; }
        public Guid? NodeId { get; set; }
        public string? Data { get; set; }
        public ICollection<Node> Children { get; set; }
        public ICollection<LocalTreeData.Models.File> Files { get; set; }
        public Node? Parent { get; set; }
        public int? Level { get; set; }
        public int? Number { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? RankId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
