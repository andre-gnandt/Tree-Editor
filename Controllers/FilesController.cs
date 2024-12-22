using Microsoft.AspNetCore.Mvc;
using LocalTreeData.Models;
using LocalTreeData.Dtos;
using LocalTreeData.Application;

namespace LocalTreeData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly NodeContext _context;

        public FilesController(NodeContext context)
        {
            _context = context;
        }

        [HttpGet("Get-Files-By-Node/{id}")]
        public async Task<ActionResult<List<FilePreview>>> GetFilesByNodeId(Guid id)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);
            
            var files = _context.Files.Where(q => q.NodeId == id && !q.IsDeleted).ToList();
            return CustomMapper.Map(files);
        }


    }
}
