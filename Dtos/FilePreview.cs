namespace LocalTreeData.Dtos
{
    public class FilePreview
    {
        public Guid Id { get; set; }
        public Guid? NodeId { get; set; }
        public byte[]? Data { get; set; }
        public string? Base64 { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public string? Type { get; set; }
    }
}
