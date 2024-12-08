namespace LocalTreeData.Dtos
{
    public class UpdateTree
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}