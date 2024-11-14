using LocalTreeData.Dtos;
using LocalTreeData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalTreeData.Interfaces;

namespace LocalTreeData.Application
{
    public class NodeService : INodeService
    {
        private readonly NodeContext _context;
        public NodeService(NodeContext nodeContext)
        { 
            _context = nodeContext;
        }

        private async Task<ActionResult<Node>> UpdateNode(Guid id, UpdateNode input)
        {
            var filesBefore = _context.Files.Where(q => q.NodeId == id && !q.IsDeleted).ToList();
            var filesAfter = CustomMapper.Map(input.Files.ToList());

            foreach (var file in filesAfter)
            {
                if (filesBefore.Find(q => q.Id == file.Id) == null)
                {
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

            Node node = CustomMapper.Map(input);

            _context.Entry(node).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return node;
        }

        public async Task<ActionResult<Node>> PutNode(Guid id, UpdateNode input)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);
           
            return await UpdateNode(id, input);
        }
    }
}
