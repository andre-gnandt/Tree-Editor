using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalTreeData.Models;
using LocalTreeData.Dtos;
using LocalTreeData.Application;

namespace LocalTreeData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreesController
    {
        private readonly EfCore.AppContext _context;

        public TreesController(EfCore.AppContext context)
        {
            _context = context;
        }

        [HttpGet("FullTree/{id}")]
        public async Task<ActionResult<FullTree>> GetFullTree(Guid id)
        {
            Node.LoadChildren(true);
            Node.LoadFiles(true);

            Tree tree = await _context.Trees.FindAsync(id);
            if (tree.IsDeleted) return new FullTree { Tree = null, Root = null };

            NodeDto root = tree.RootId != null ? CustomMapper.Map(await _context.Nodes.FindAsync(tree.RootId)) : null;
            
            return new FullTree { Tree = CustomMapper.Map(tree), Root = root};
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
        public async Task<ActionResult<TreeDto>> GetTreeDetails(Guid id)
        {

            return CustomMapper.Map(await  _context.Trees.FindAsync(id));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TreeDto>>> GetTreeList()
        {

            return CustomMapper.Map( _context.Trees.Where(q => !q.IsDeleted).OrderBy(q => q.Name).ToList());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TreeDto>> UpdateTreeDetails(Guid id, UpdateTree input)
        {
            Tree tree = (await _context.Trees.FindAsync(id));
            tree.Name = input.Name;
            tree.Description = input.Description;
            _context.Entry(tree).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Concurrent Update of Tree Entity Record with Id = " + id.ToString());
            }

            return CustomMapper.Map(tree);
        }

        [HttpPost]
        public async Task<ActionResult<TreeDto>> CreateTree(CreateTree input)
        {
            Tree tree = CustomMapper.Map(input);

            _context.Trees.Add(tree);
            await _context.SaveChangesAsync();

            return CustomMapper.Map(tree);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TreeDto>> DeleteTree(Guid id)
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
                throw new DbUpdateConcurrencyException("Concurrent Update of Tree Entity Record with Id = "+id.ToString());
            }

            return CustomMapper.Map(tree);
        }
    }
}
