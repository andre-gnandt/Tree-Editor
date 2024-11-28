using LocalTreeData.Models;

namespace LocalTreeData.Dtos
{
    public class FullTree
    {
        public Tree Tree { get; set; }
        public NodeDto Root { get; set; }
    }
}
