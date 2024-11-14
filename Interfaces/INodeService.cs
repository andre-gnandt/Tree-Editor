using LocalTreeData.Dtos;
using LocalTreeData.Models;
using Microsoft.AspNetCore.Mvc;

namespace LocalTreeData.Interfaces
{
    public interface INodeService
    {  
        public Task<ActionResult<Node>> PutNode(Guid id, UpdateNode input);
    }
}
