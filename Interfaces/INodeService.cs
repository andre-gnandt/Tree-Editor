using LocalTreeData.Dtos;
using LocalTreeData.Models;
using Microsoft.AspNetCore.Mvc;

namespace LocalTreeData.Interfaces
{
    public interface INodeService
    {  
        public Task<ActionResult<Node>> PutNode(Guid id, UpdateNode input);
        public Task<ActionResult<List<Node>>> UpdateMany(Guid id, List<UpdateNode> inputList);
        public Task<ActionResult<Node>> Create(CreateNode input);
        public Task<ActionResult<Node>> CreateRoot(CreateNode input);
    }
}
