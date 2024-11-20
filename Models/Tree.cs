namespace LocalTreeData.Models
{
    public class Tree
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid? RootId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
