using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocalTreeData.Models;
using LocalTreeData.Dtos;
using LocalTreeData.Application;
using System.Net;
using System.Text;

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
