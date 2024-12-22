namespace LocalTreeData.Models
{
    public class ConfigType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; } = default(DateTime?);
        public DateTime? UpdatedDate { get; } = default(DateTime?);
        public string? Value { get; set; }
    }
}
