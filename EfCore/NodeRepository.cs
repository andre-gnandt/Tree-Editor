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

        public async Task<List<Models.File>> UpdateNodeFilesAsync(Node input, List<Models.File> filesAfter)
        {
            var filesBefore = _context.Files.Where(q => q.NodeId == input.Id && !q.IsDeleted).ToList();

            foreach (var file in filesAfter)
            {
                if (filesBefore.Find(q => q.Id == file.Id) == null)
                {
                    file.Id = Guid.NewGuid();
                    _context.Files.Add(file);
                    await _context.SaveChangesAsync();

                    if (input.ThumbnailId == file.Name) input.ThumbnailId = file.Id.ToString();
                }
            }

            foreach (var file in filesBefore)
            {
                if (filesAfter.Find(q => q.Id == file.Id) == null)
                {
                    file.IsDeleted = true;
                    _context.Entry(file).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }

            return filesAfter;
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
