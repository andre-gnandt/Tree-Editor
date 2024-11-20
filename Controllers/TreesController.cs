using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalTreeData.Models;
using LocalTreeData.Dtos;
using LocalTreeData.Application;
using LocalTreeData.Interfaces;
using System.Linq;
using System.Xml.Linq;

namespace LocalTreeData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreesController
    {
        private readonly NodeContext _context;

        public TreesController(NodeContext context)
        {
            _context = context;
        }

        [HttpGet("FullTree/{id}")]
        public async Task<ActionResult<FullTree>> GetFullTree(Guid id)
        {
            Node.LoadChildren(true);
            Node.LoadFiles(true);

            Tree tree = await _context.Trees.FindAsync(id);
            NodeDto root = tree.RootId != null ? CustomMapper.Map(await _context.Nodes.FindAsync(tree.RootId)) : null;
            
            return new FullTree { Tree = tree, Root = root};
        }

        [HttpGet("Tree/{id}")]
        public async Task<ActionResult<NodeDto>> GetTree(Guid id)
        {
            Node.LoadChildren(true);
            Node.LoadFiles(true);

            Tree tree = await _context.Trees.FindAsync(id);
            NodeDto root = tree.RootId != null ? CustomMapper.Map(await _context.Nodes.FindAsync(tree.RootId)) : null;
            return root;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tree>> GetTreeDetails(Guid id)
        {

            return await  _context.Trees.FindAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tree>>> GetTreeList()
        {

            return _context.Trees.Where(q => !q.IsDeleted).OrderBy(q => q.Name).ToList();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Tree>> UpdateTreeDetails(Guid id, Tree tree)
        {
            _context.Entry(tree).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return tree;
        }

        [HttpPost]
        public async Task<ActionResult<Tree>> CreateTree(CreateTree input)
        {
            Tree tree = CustomMapper.Map(input);

            _context.Trees.Add(tree);
            await _context.SaveChangesAsync();

            return tree;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Tree>> DeleteTree(Guid id)
        {
            Tree tree = await _context.Trees.FindAsync(id);
            tree.IsDeleted = true;

            _context.Entry(tree).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return tree;
        }
    }
}
