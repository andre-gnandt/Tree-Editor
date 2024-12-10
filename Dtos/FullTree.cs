using LocalTreeData.Models;

namespace LocalTreeData.Dtos
{
    public class FullTree
    {
        public TreeDto Tree { get; set; }
        public NodeDto Root { get; set; }
    }
}
