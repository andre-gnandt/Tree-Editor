﻿using LocalTreeData.Dtos;
using LocalTreeData.Models;
using LocalTreeData.ApplicationInterfaces;
using LocalTreeData.EfCore;
using LocalTreeData.EfCoreInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace LocalTreeData.Application
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly ITreeRepository _treeRepository;

        public NodeService(INodeRepository nodeRepository, ITreeRepository treeRepository)
        { 
            _nodeRepository = nodeRepository;
            _treeRepository = treeRepository;
        }

        public async Task<ActionResult<NodeDto>> GetNodeAsync(Guid id)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);
            return CustomMapper.Map(await _nodeRepository.GetNodeAsync(id));
        }

        public async Task<ActionResult<IEnumerable<NodeDto>>> GetTreesAsync()
        {
            Node.LoadFiles(true);
            Node.LoadChildren(true);
            return CustomMapper.Map((await _nodeRepository.GetTreesAsync()).ToList());
        }

        public async Task<ActionResult<IEnumerable<NodeDto>>> GetNodesAsync()
        {
            Node.LoadFiles(false);
            Node.LoadChildren(false);
            return CustomMapper.Map((await _nodeRepository.GetNodesAsync()).ToList());
        }

        private async Task<NodeDto> UpdateNode(Guid id, UpdateNode input)
        {
            var filesAfter = CustomMapper.Map(input.Files.ToList());
            Node node = CustomMapper.Map(input);
            node.Files = [];
            
            var files = CustomMapper.Map(await _nodeRepository.UpdateNodeFilesAsync(node, filesAfter));
            var nodeDto = CustomMapper.Map(await _nodeRepository.UpdateAsync(node));
            nodeDto.Files = files;
            
            return nodeDto;
        }

        public async Task<ActionResult<List<NodeDto>>> UpdateMany(Guid treeId, List<UpdateNode> inputList)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);

            List<NodeDto> updatedNodes = new List<NodeDto>();
            foreach (var input in inputList)
            {
                if (input.TreeId != treeId)
                {
                    throw new ArgumentOutOfRangeException("Node does not belong to tree for updates.");
                }

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
            var filesAfter = input.Files.ToList();
            Node node = CustomMapper.Map(input);
            await _nodeRepository.CreateAsync(node);

            foreach (var file in filesAfter)
            {
                file.NodeId = node.Id;
            }
            var files = CustomMapper.Map(await _nodeRepository.UpdateNodeFilesAsync(node, CustomMapper.Map(filesAfter)));
            if (input.ThumbnailId != null) await _nodeRepository.UpdateAsync(node);
            var nodeDto = CustomMapper.Map(node);
            nodeDto.Files = files;

            return nodeDto;
        }

        public async Task<ActionResult<NodeDto>> CreateRoot(CreateNode input)
        {
            Node.LoadChildren(false);
            Node.LoadFiles(true);

            Tree tree = await _treeRepository.GetAsync((Guid)input.TreeId);
            Node oldRootNode = tree.RootId != null ? (await _nodeRepository.GetNodeAsync((Guid)tree.RootId)) : null;

            NodeDto newRoot = await CreateNode(input);

            if (oldRootNode != null)
            {
                oldRootNode.NodeId = newRoot.Id;
                await _nodeRepository.UpdateAsync(oldRootNode);
            }
            
            tree.RootId = newRoot.Id;
            await _treeRepository.UpdateAsync(tree);

            return newRoot;
        }

        public async Task<ActionResult<NodeDto>> Create(CreateNode input)
        {
            Node.LoadChildren(false);
            Node.LoadFiles(true);

            return await CreateNode(input);
        }

        public async Task<ActionResult<NodeDto>> DeleteNode(Guid parentId, UpdateNode input)
        {
            Node.LoadFiles(false);
            Node.LoadChildren(false);

            var children = input.Children;
            input.Children = [];

            
            Node node = await _nodeRepository.DeleteAsync(CustomMapper.Map(input));
            
            foreach (UpdateNode child in children)
            {
                child.NodeId = parentId;
                await _nodeRepository.UpdateAsync(CustomMapper.Map(child));
            }

            return CustomMapper.Map(node);
        }

        public async Task<ActionResult<NodeDto>> DeleteCascade(Guid id)
        {
            Node.LoadFiles(false);
            Node.LoadChildren(true);

            Node node = await _nodeRepository.DeleteAsync(id);
            await _nodeRepository.DeleteTreeAsync(node);

            return CustomMapper.Map(node);

        }
    }
}
