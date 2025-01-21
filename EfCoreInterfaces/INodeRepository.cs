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
        public Task DeleteTreeAsync(Node node);
        public Task<Node> UpdateAsync(Node node);
        public Task<Node> CreateAsync(Node node);
        public Task DeleteFile(Models.File file);
        public Task<List<Models.File>> GetFilesByNodeId(Guid NodeId);
        public Task<Models.File> CreateFile(Models.File file);
    }
}
