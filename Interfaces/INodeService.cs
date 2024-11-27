using LocalTreeData.Dtos;
using LocalTreeData.Models;
using Microsoft.AspNetCore.Mvc;

namespace LocalTreeData.Interfaces
{
    public interface INodeService
    {  
        public Task<ActionResult<NodeDto>> PutNode(Guid id, UpdateNode input);
        public Task<ActionResult<List<NodeDto>>> UpdateMany(Guid id, List<UpdateNode> inputList);
        public Task<ActionResult<NodeDto>> Create(CreateNode input);
        public Task<ActionResult<NodeDto>> CreateRoot(CreateNode input);
        public Task<ActionResult<NodeDto>> DeleteNode(Guid id);
        public Task<ActionResult<NodeDto>> DeleteCascade(Guid id);
    }
}
