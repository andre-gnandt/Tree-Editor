using LocalTreeData.Models;

namespace LocalTreeData.Dtos
{
    public class NodeDto
    {

        public Guid Id { get; set; }
        public Guid? NodeId { get; set; }
        public Guid? TreeId { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? Data { get; set; }
        public ICollection<NodeDto> Children { get; set; }
        public ICollection<FileDto> Files { get; set; } = new List<FileDto>();
        public Node? Parent { get; set; }
        public int? Level { get; set; }
        public int? Number { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid? RankId { get; set; }
        public string? ThumbnailId { get; set; }
    }
}
