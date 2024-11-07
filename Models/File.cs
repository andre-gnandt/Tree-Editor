using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LocalTreeData.Models
{
    public class File
    {
        public Guid Id { get; set; }
        public Node? Node { get; set; }
        public Guid? NodeId { get; set; }
        public string? Data { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public string? Type { get; set; }
        public bool IsDeleted { get; set; }
    }
}