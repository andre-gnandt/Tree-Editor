using LocalTreeData.Models;

namespace LocalTreeData.Dtos
{
    public class NodeDto
    {

        public Guid Id { get; set; }
        public Guid? NodeId { get; set; }
        public Guid? TreeId { get; set; }
        public string? Data { get; set; }
        public ICollection<NodeDto> Children { get; set; }
        public ICollection<FilePreview> Files { get; set; } = new List<FilePreview>();
        public Node? Parent { get; set; }
        public int? Level { get; set; }
        public int? Number { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? RankId { get; set; }
        public string? ThumbnailId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
