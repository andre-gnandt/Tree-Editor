﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalTreeData.Models;
using LocalTreeData.Dtos;
using LocalTreeData.Application;
using LocalTreeData.ApplicationInterfaces;

namespace LocalTreeData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase, INodeService
    {
        private readonly INodeService _nodeService;

        public NodesController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet("Trees")]
        public async Task<ActionResult<IEnumerable<NodeDto>>> GetTreesAsync()
        {
            return await _nodeService.GetTreesAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NodeDto>>> GetNodesAsync()
        {
            return await _nodeService.GetNodesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NodeDto>> GetNodeAsync(Guid id)
        {
            var node = await _nodeService.GetNodeAsync(id);

            if (node == null)
            {
                return NotFound();
            }

            return node;
        }

        [HttpPut("Many/{treeId}")]
        public async Task<ActionResult<List<NodeDto>>> UpdateMany(Guid treeId, List<UpdateNode> input)
        {
            return await _nodeService.UpdateMany(treeId, input);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NodeDto>> PutNode(Guid id, UpdateNode input)
        {   
            if (id != input.Id)
            {
                return BadRequest();
            }
            
            return await _nodeService.PutNode(id, input);
        }

        [HttpPost]
        public async Task<ActionResult<NodeDto>> Create(CreateNode input)
        {
            return await _nodeService.Create(input);
        }

        [HttpPost("Root")]
        public async Task<ActionResult<NodeDto>> CreateRoot(CreateNode input)
        { 
            return await _nodeService.CreateRoot(input);
        }

        [HttpDelete("Delete-One/{parentId}")]
        public async Task<ActionResult<NodeDto>> DeleteNode(Guid parentId, UpdateNode node)
        {
            return await _nodeService.DeleteNode(parentId, node);
        }

        [HttpDelete("Delete-Cascade{id}")]
        public async Task<ActionResult<NodeDto>> DeleteCascade(Guid id)
        {
            return await _nodeService.DeleteCascade(id);
        }
    }
}
