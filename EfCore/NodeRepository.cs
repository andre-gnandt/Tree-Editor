using LocalTreeData.EfCoreInterfaces;
using LocalTreeData.Models;
using Microsoft.EntityFrameworkCore;

namespace LocalTreeData.EfCore
{
    public class NodeRepository : INodeRepository
    {
        private readonly AppContext _context;

        public NodeRepository(AppContext nodeContext)
        {
            _context = nodeContext;
        }

        public async Task<Node> GetNodeAsync(Guid id)
        { 
            Node node = await _context.Nodes.FindAsync(id);
            if (node != null && node.IsDeleted) node = null;

            return node;
        }

        public async Task<IEnumerable<Node>> GetTreesAsync()
        {
            return _context.Nodes.Where(q => q.NodeId == null && !q.IsDeleted);
        }

        public async Task<IEnumerable<Node>> GetNodesAsync()
        {
            return _context.Nodes.Where(q => !q.IsDeleted);
        }

        public async Task<Node> UpdateAsync(Node node)
        {
            _context.Entry(node).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException("Concurrent Update of Node Entity Record with Id = " + node.Id.ToString());
            }

            return node;
        }

        public async Task<Node> CreateAsync(Node node)
        {
            node.Files = [];
            _context.Nodes.Add(node);
            await _context.SaveChangesAsync();

            return node;
        }

        public async Task<Models.File> CreateFile(Models.File file)
        {
            file.Id = Guid.NewGuid();
            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return file;
        }

        public async Task DeleteFile(Models.File file)
        {
            file.IsDeleted = true;
            _context.Entry(file).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Models.File>> GetFilesByNodeId(Guid NodeId)
        { 
            return _context.Files.Where(q => q.NodeId == NodeId && !q.IsDeleted).ToList();
        }

        public async Task DeleteTreeAsync(Node node)
        {
            foreach (var child in node.Children)
            {
                await DeleteAsync(child.Id);
                await DeleteTreeAsync(child);
            }
        }

        public async Task<Node> DeleteAsync(Guid id)
        {
            var node = await _context.Nodes.FindAsync(id);
            node.IsDeleted = true;
            return await UpdateAsync(node);
        }

        public async Task<Node> DeleteAsync(Node node)
        {
            node.IsDeleted = true;
            return await UpdateAsync(node);
        }
    }
}
