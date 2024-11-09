using LocalTreeData.Models;
using LocalTreeData.Dtos;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Buffers.Text;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;

namespace LocalTreeData.Application
{
    public class CustomMapper
    {
        public CustomMapper() { }

        public static NodeDto Map(Node node)
        {
            return new NodeDto
            {
                Id = node.Id,
                NodeId = node.NodeId,
                Title = node.Title,
                Description = node.Description,
                Data = node.Data,
                Number = node.Number,
                RankId = node.RankId,
                Children = node.Children,
                ThumbnailId = node.ThumbnailId,
                IsDeleted = node.IsDeleted,
                Level = node.Level,
                Files = Map(node.Files.ToList())
            };
        }

        public static List<NodeDto> Map(List<Node> nodes)
        {
            List<NodeDto> nodelist = new List<NodeDto>();

            foreach (var node in nodes)
            { 
                nodelist.Add(Map(node));
            }

            return nodelist;
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
                Type = file.Type,
                IsDeleted = file.IsDeleted,
            };
        }

        public static Models.File Map(FilePreview filePreview)
        {
            return new Models.File
            {
                Id = filePreview.Id,
                NodeId = filePreview.NodeId,
                Data = filePreview.Data != null && filePreview.Data.Length > 0 ? filePreview.Data : null,   
                Name = filePreview.Name,
                Description = filePreview.Description,
                Size = filePreview.Size,
                Type = filePreview.Type,
                IsDeleted = filePreview.IsDeleted,
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
                Data = node.Data,
                Children = node.Children,
                Files = Map(node.Files.ToList()),
                Level = node.Level,
                Number = node.Number,
                Title = node.Title,
                Description = node.Description,
                RankId = node.RankId,
                ThumbnailId = node.ThumbnailId,
                IsDeleted = node.IsDeleted,
            }; 
        }

        public static Node Map(CreateNode node)
        {
            return new Node
            {
                Id = Guid.Empty,
                NodeId = node.NodeId,
                Data = node.Data,
                Children = node.Children,
                Files = Map(node.Files.ToList()),
                Level = node.Level,
                Number = node.Number,
                Title = node.Title,
                Description = node.Description,
                RankId = node.RankId,
                ThumbnailId = node.ThumbnailId,
                IsDeleted = node.IsDeleted,
            };
        }
    }
}
