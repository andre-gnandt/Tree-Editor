using LocalTreeData.Models;

namespace LocalTreeData.EfCoreInterfaces
{
    public interface ITreeRepository
    {
        public Task<Tree> GetAsync(Guid id);
        public Task<Tree> UpdateAsync(Tree tree);
    }
}
