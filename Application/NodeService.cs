using LocalTreeData.Dtos;
using LocalTreeData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalTreeData.Interfaces;
using Microsoft.CodeAnalysis;
using System.Xml.Linq;

namespace LocalTreeData.Application
{
    public class NodeService : INodeService
    {
        private readonly NodeContext _context;
        public NodeService(NodeContext nodeContext)
        { 
            _context = nodeContext;
        }

        private async Task<Node> Update(Node node)
        {
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

        private async Task<List<FilePreview>> UpdateNodeFiles(Node input, List<Models.File> filesAfter)
        {
            var filesBefore = _context.Files.Where(q => q.NodeId == input.Id && !q.IsDeleted).ToList();

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

            return CustomMapper.Map(filesAfter);
        }

        private async Task<NodeDto> UpdateNode(Guid id, UpdateNode input)
        {
            var filesAfter = CustomMapper.Map(input.Files.ToList());
            Node node = CustomMapper.Map(input);
            
            var files = await UpdateNodeFiles(node, filesAfter);
            var nodeDto = CustomMapper.Map(await Update(node));
            nodeDto.Files = files;
            
            return nodeDto;
        }

        public async Task<ActionResult<List<NodeDto>>> UpdateMany(Guid id, List<UpdateNode> inputList)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);

            List<NodeDto> updatedNodes = new List<NodeDto>();
            foreach (var input in inputList)
            {
                updatedNodes.Add(await UpdateNode(input.Id, input));
            }

            return updatedNodes;
        }

        public async Task<ActionResult<NodeDto>> PutNode(Guid id, UpdateNode input)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);
           
            return await UpdateNode(id, input);
        }

        private async Task<NodeDto> CreateNode(CreateNode input)
        {
            var filesAfter = CustomMapper.Map(input.Files.ToList());
            Node node = CustomMapper.Map(input);
            
            _context.Nodes.Add(node);
            await _context.SaveChangesAsync();

            foreach (var file in filesAfter)
            {
                file.NodeId = node.Id;
            }
            var files = await UpdateNodeFiles(node, filesAfter);
            if (input.ThumbnailId != null) await Update(node);
            var nodeDto = CustomMapper.Map(node);
            nodeDto.Files = files;

            return nodeDto;
        }

        public async Task<ActionResult<NodeDto>> CreateRoot(CreateNode input)
        {
            Node.LoadChildren(false);
            Node.LoadFiles(true);

            Tree tree = await _context.Trees.FindAsync(input.TreeId);
            Node oldRootNode = tree.RootId != null ? await _context.Nodes.FindAsync(tree.RootId) : null;

            NodeDto newRoot = await CreateNode(input);

            if (oldRootNode != null)
            {
                oldRootNode.NodeId = newRoot.Id;
                await Update(oldRootNode);
            }
            
            tree.RootId = newRoot.Id;
            _context.Entry(tree).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return newRoot;
        }

        public async Task<ActionResult<NodeDto>> Create(CreateNode input)
        {
            Node.LoadChildren(false);
            Node.LoadFiles(true);

            return await CreateNode(input);
        }

        private async Task DeleteTree(Node node)
        {
            foreach (var child in node.Children)
            {
                await Delete(child.Id);
                await DeleteTree(child);
            }
        }

        private async Task<Node> Delete(Guid id)
        {
            var node = await _context.Nodes.FindAsync(id);
            node.IsDeleted = true;
            return await Update(node);
        }
        
        public async Task<ActionResult<NodeDto>> DeleteNode(Guid id)
        {
            Node.LoadFiles(false);
            Node.LoadChildren(true);
            
            Node tree = _context.Nodes.Find(id);
            Node node = await Delete(id);

            foreach(Node child in tree.Children)
            {
                child.NodeId = tree.NodeId;
                await Update(child);
            }

            return CustomMapper.Map(node);
        }

        public async Task<ActionResult<NodeDto>> DeleteCascade(Guid id)
        {
            Node.LoadFiles(false);
            Node.LoadChildren(true);

            Node tree = _context.Nodes.Find(id);
            Node node = await Delete(id);

            await DeleteTree(tree);

            return CustomMapper.Map(node);

        }
    }
}
