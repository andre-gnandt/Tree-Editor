using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalTreeData.Models;

namespace LocalTreeData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private readonly NodeContext _context;

        public NodesController(NodeContext context)
        {
            _context = context;
        }

        private void SetParentNodes(Node root)
        {
            foreach (var child in root.Children)
            {
                child.Parent = root;
                SetParentNodes(child);
            }
        }

        [HttpGet("Trees")]
        public async Task<ActionResult<IEnumerable<Node>>> GetTrees()
        {
            //_context.ChangeTracker.LazyLoadingEnabled = true;
            Node.LoadEntities(true);

            List<Node> trees = await _context.Nodes.Where(q => q.Level == 0).ToListAsync();
            if (trees != null)
            {
                foreach (var root in trees)
                {
                    SetParentNodes(root);
                }
            }

            return trees;
        }

        // GET: api/Nodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Node>>> GetNodes()
        {
            Node.LoadEntities(false);
            return await _context.Nodes.ToListAsync();
        }

        // GET: api/Nodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Node>> GetNode(Guid id)
        {
            Node.LoadEntities(false);
            var node = await _context.Nodes.FindAsync(id);

            if (node == null)
            {
                return NotFound();
            }

            return node;
        }

        // PUT: api/Nodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNode(Guid id, Node node)
        {
            Node.LoadEntities(false);
            if (id != node.Id)
            {
                return BadRequest();
            }

            _context.Entry(node).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Nodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Node>> PostNode(Node node)
        {
            Node.LoadEntities(false);
            _context.Nodes.Add(node);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Node), new { id = node.Id }, node);
        }

        // DELETE: api/Nodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNode(Guid id)
        {
            Node.LoadEntities(false);
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
            Node.LoadEntities(false);
            return _context.Nodes.Any(e => e.Id == id);
        }
    }
}
