using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalTreeData.Models;
using LocalTreeData.Dtos;
using LocalTreeData.Application;
using LocalTreeData.Interfaces;

namespace LocalTreeData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase, INodeService
    {
        private readonly NodeContext _context;
        private readonly INodeService _nodeService;

        public NodesController(NodeContext context, INodeService nodeService)
        {
            _context = context;
            _nodeService = nodeService;
        }

        [HttpGet("Trees")]
        public async Task<ActionResult<IEnumerable<NodeDto>>> GetTrees()
        {
            //_context.ChangeTracker.LazyLoadingEnabled = true;
            Node.LoadChildren(true);
            Node.LoadFiles(true);

            List<Node> trees = await _context.Nodes.Where(q => q.NodeId == null && !q.IsDeleted).ToListAsync();
            return CustomMapper.Map(trees);
        }

        // GET: api/Nodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NodeDto>>> GetNodes()
        {
            Node.LoadFiles(false);
            Node.LoadChildren(false);
            return (CustomMapper.Map(await _context.Nodes.Where(q => !q.IsDeleted).ToListAsync()));
        }

        [HttpPut("ImageTest/{id}")]
        public async Task<ActionResult<string>> GetImageBase64Test(Guid id, Test test)
        {
            
            string base64 = Convert.ToBase64String(test.Image);
            return base64;
           

            return test.StringVar;
        }

        // GET: api/Nodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NodeDto>> GetNode(Guid id)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);
            var node = await _context.Nodes.FindAsync(id);

            if (node == null)
            {
                return NotFound();
            }

            return CustomMapper.Map(node);
        }

        // PUT: api/Nodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Node>> PutNode(Guid id, UpdateNode input)
        {   
            if (id != input.Id)
            {
                return BadRequest();
            }
            
            return await _nodeService.PutNode(id, input);
        }

        // POST: api/Nodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Node>> PostNode(CreateNode input)
        {
            Node.LoadChildren(false);
            Node.LoadFiles(true);

            Node node = CustomMapper.Map(input);
            _context.Nodes.Add(node);
            await _context.SaveChangesAsync();

            return node;
        }

        // DELETE: api/Nodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNode(Guid id)
        {
            Node.LoadChildren(false);
            var node = await _context.Nodes.FindAsync(id);
            if (node == null)
            {
                return NotFound();
            }

            _context.Nodes.Remove(node);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NodeExists(Guid id)
        {
            Node.LoadChildren(false);
            return _context.Nodes.Any(e => e.Id == id);
        }

        private bool FileExists(Guid id)
        {
            return _context.Files.Any(e => e.Id == id);
        }
    }
}
