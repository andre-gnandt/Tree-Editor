using LocalTreeData.EfCoreInterfaces;
using LocalTreeData.Models;
using Microsoft.EntityFrameworkCore;

namespace LocalTreeData.EfCore
{
    public class TreeRepository : ITreeRepository
    {
        private readonly AppContext _context;
        public TreeRepository(AppContext nodeContext)
        {
            _context = nodeContext;
        }

        public async Task<Tree> GetAsync(Guid id)
        {
            Tree tree = await _context.Trees.FindAsync(id);
            if (tree != null && tree.IsDeleted) return null;

            return tree;
        }

        public async Task<Tree> UpdateAsync(Tree tree)
        {
            _context.Entry(tree).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return tree;
        }
    }
}
