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
        private readonly EfCore.AppContext _context;

        public FilesController(EfCore.AppContext context)
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

        [HttpGet("{id}")]
        public async Task<ActionResult<FilePreview>> GetFile(Guid id)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);

            var file = await _context.Files.FindAsync(id);
            return CustomMapper.Map(file);
        }
    }
}
