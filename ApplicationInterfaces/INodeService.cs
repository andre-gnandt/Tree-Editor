using LocalTreeData.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LocalTreeData.ApplicationInterfaces
{
    public interface INodeService
    {  
        public Task<ActionResult<NodeDto>> PutNode(Guid id, UpdateNode input);
        public Task<ActionResult<List<NodeDto>>> UpdateMany(Guid id, List<UpdateNode> inputList);
        public Task<ActionResult<NodeDto>> Create(CreateNode input);
        public Task<ActionResult<NodeDto>> CreateRoot(CreateNode input);
        public Task<ActionResult<NodeDto>> DeleteNode(Guid parentId, UpdateNode node);
        public Task<ActionResult<NodeDto>> DeleteCascade(Guid id);
        public Task<ActionResult<NodeDto>> GetNodeAsync(Guid id);
        public Task<ActionResult<IEnumerable<NodeDto>>> GetTreesAsync();
        public Task<ActionResult<IEnumerable<NodeDto>>> GetNodesAsync();
    }
}
