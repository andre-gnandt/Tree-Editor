using LocalTreeData.Models;
using LocalTreeData.Dtos;

namespace LocalTreeData.Application
{
    public class CustomMapper
    {
        public CustomMapper() { }

        public static Node Map(UpdateNode node) 
        {
            List<LocalTreeData.Models.File> cloneFiles = new List<LocalTreeData.Models.File>();

            foreach (var file in node.Files.ToList())
            {
                cloneFiles.Add(new Models.File { 
                    Id = file.Id,
                    NodeId = file.NodeId,
                    Data = file.Data,
                    Name = file.Name,
                    Description = file.Description,
                    Size = file.Size,
                    Type = file.Type,
                    IsDeleted = file.IsDeleted
                });
            }

            return new Node { 
                Id = node.Id,
                NodeId = node.NodeId,
                Data = node.Data,
                Children = node.Children,
                Files = cloneFiles,
                Level = node.Level,
                Number = node.Number,
                Title = node.Title,
                Description = node.Description,
                RankId = node.RankId,
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
                Files = node.Files,
                Level = node.Level,
                Number = node.Number,
                Title = node.Title,
                Description = node.Description,
                RankId = node.RankId,
                IsDeleted = node.IsDeleted,
            };
        }
    }
}
