﻿using LocalTreeData.Models;
using LocalTreeData.Dtos;

namespace LocalTreeData.Application
{
    public class CustomMapper
    {
        public CustomMapper() { }


        public static List<NodeDto> Map(List<Node> nodes)
        {
            List<NodeDto> nodelist = new List<NodeDto>();

            foreach (var node in nodes)
            { 
                nodelist.Add(Map(node));
            }

            return nodelist;
        }

        //Map full tree
        public static NodeDto Map(Node node)
        {
            NodeDto nodeDto = new NodeDto
            {
                Id = node.Id,
                NodeId = node.NodeId,
                TreeId = node.TreeId,
                Title = node.Title,
                Description = node.Description,
                Country = node.Country,
                Region = node.Region,
                Data = node.Data,
                Number = node.Number,
                RankId = node.RankId,
                ThumbnailId = node.ThumbnailId,
                Level = node.Level,
                Files = MapDto(node.Files.ToList())
            };

            nodeDto.Children = new List<NodeDto>();
            foreach (var child in node.Children)
            { 
                nodeDto.Children.Add(Map(child));
            }

            return nodeDto;
        }
        public static FilePreview Map(Models.File file)
        {
            return new FilePreview
            {
                Id = file.Id,
                NodeId = file.NodeId,
                Data = null,
                Base64 = file.Data != null && file.Data.Length > 0 ? "data:image/png;base64,"+Convert.ToBase64String(file.Data) : null,
                Name = file.Name,
                Description = file.Description, 
                Size = file.Size,
                Type = file.Type
            };
        }

        public static FileDto MapDto(FilePreview file)
        {
            return new FileDto
            {
                Id = file.Id,
                Name = file.Name
            };
        }

        public static List<FileDto> MapDto(List<FilePreview> fileList)
        {
            List<FileDto> previewList = new List<FileDto>();

            foreach (var file in fileList)
            {
                previewList.Add(MapDto(file));
            }

            return previewList;
        }

        public static FileDto MapDto(Models.File file)
        {
            return new FileDto
            {
                Id = file.Id,
                Name = file.Name
            };
        }

        public static List<FileDto> MapDto(List<Models.File> fileList)
        {
            List<FileDto> previewList = new List<FileDto>();

            foreach (var file in fileList)
            {
                previewList.Add(MapDto(file));
            }

            return previewList;
        }

        public static Models.File Map(FilePreview filePreview)
        {
            return new Models.File
            {
                Id = filePreview.Id,
                NodeId = filePreview.NodeId,
                Data = filePreview.Data != null && filePreview.Data.Length > 0 ? Convert.FromBase64String(filePreview.Data.Substring(filePreview.Data.IndexOf("base64,")+7)) : null,   
                Name = filePreview.Name,
                Description = filePreview.Description,
                Size = filePreview.Size,
                Type = filePreview.Type,
                IsDeleted = false,
            };
        }

        public static List<FilePreview> Map(List<Models.File> fileList)
        { 
            List<FilePreview> previewList = new List<FilePreview>();

            foreach (var file in fileList)
            {
                previewList.Add(Map(file));
            }

            return previewList;
        }

        public static List<Models.File> Map(List<FilePreview> previewList)
        {
            List<Models.File> fileList = new List<Models.File>();

            foreach (var file in previewList)
            {
                fileList.Add(Map(file));
            }

            return fileList;
        }

        public static List<Node> Map(List<CreateNode> nodes)
        { 
            List<Node> list = new List<Node>();

            foreach (CreateNode node in nodes)
            { 
                list.Add(Map(node));
            }

            return list;
        }

        public static Node Map(CreateNode node)
        {

            return new Node
            {
                Id = Guid.NewGuid(),
                NodeId = node.NodeId,
                TreeId = node.TreeId,
                Country = node.Country,
                Region = node.Region,
                Data = node.Data,
                Children = Map(node.Children.ToList()),
                Files = Map(node.Files.ToList()),
                Level = node.Level,
                Number = node.Number,
                Title = node.Title,
                Description = node.Description,
                RankId = node.RankId,
                ThumbnailId = node.ThumbnailId,
                IsDeleted = false
            };
        }

        public static List<Node> Map(List<UpdateNode> nodes)
        {
            List<Node> list = new List<Node>();

            foreach (UpdateNode node in nodes)
            {
                list.Add(Map(node));
            }

            return list;
        }

        public static Node Map(UpdateNode node) 
        {
            /*
            List<FilePreview> cloneFiles = new List<FilePreview>();

            foreach (var file in node.Files.ToList())
            {
                cloneFiles.Add(new FilePreview { 
                    Id = file.Id,
                    NodeId = file.NodeId,
                    Data = file.Data,
                    Base64 = file.Base64,
                    Name = file.Name,
                    Description = file.Description,
                    Size = file.Size,
                    Type = file.Type,
                    IsDeleted = file.IsDeleted
                });
            }
            */

            return new Node { 
                Id = node.Id,
                NodeId = node.NodeId,
                Country = node.Country,
                Region = node.Region,
                TreeId = node.TreeId,
                Data = node.Data,
                Children = Map(node.Children.ToList()),
                Files = Map(node.Files.ToList()),
                Level = node.Level,
                Number = node.Number,
                Title = node.Title,
                Description = node.Description,
                RankId = node.RankId,
                ThumbnailId = node.ThumbnailId,
                IsDeleted = false
            }; 
        }

        public static Tree Map(CreateTree tree)
        {

            return new Tree
            {
                Id = Guid.NewGuid(),
                Name = tree.Name,
                Description = tree.Description,
                RootId = null,
                IsDeleted = false
            };
        }

        public static TreeDto Map(Tree tree)
        {
            return new TreeDto
            {
                Id = tree.Id,
                Name = tree.Name,
                Description= tree.Description,
                RootId= tree.RootId
            };
        }

        public static Tree Map(TreeDto tree)
        {
            return new Tree
            {
                Id = tree.Id,
                Name = tree.Name,
                Description = tree.Description,
                RootId = tree.RootId,
                IsDeleted = false
            };
        }

        public static List<TreeDto> Map(List<Tree> trees)
        { 
            List<TreeDto> treeDtos = new List<TreeDto>();
            foreach (Tree tree in trees)
            { 
                treeDtos.Add(Map(tree));
            }

            return treeDtos;
        }
    }
}
