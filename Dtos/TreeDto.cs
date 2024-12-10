namespace LocalTreeData.Dtos
{
    public class TreeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid? RootId { get; set; }
    }
}