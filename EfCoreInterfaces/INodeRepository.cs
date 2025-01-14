using LocalTreeData.Models;
using Microsoft.AspNetCore.Mvc;

namespace LocalTreeData.EfCoreInterfaces
{
    public interface INodeRepository
    {
        public Task<Node> GetNodeAsync(Guid id);
        public Task<Node> DeleteAsync(Guid id);
        public Task<Node> DeleteAsync(Node node);
        public Task<IEnumerable<Node>> GetTreesAsync();
        public Task<IEnumerable<Node>> GetNodesAsync();
        public Task<List<Models.File>> UpdateNodeFilesAsync(Node input, List<Models.File> filesAfter);
        public Task DeleteTreeAsync(Node node);
        public Task<Node> UpdateAsync(Node node);
        public Task<Node> CreateAsync(Node node);
    }
}
