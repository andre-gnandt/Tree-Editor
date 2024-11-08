﻿using System;
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
    public class NodesController : ControllerBase
    {
        private readonly NodeContext _context;

        public NodesController(NodeContext context)
        {
            _context = context;
        }

        [HttpGet("Trees")]
        public async Task<ActionResult<IEnumerable<Node>>> GetTrees()
        {
            //_context.ChangeTracker.LazyLoadingEnabled = true;
            Node.LoadChildren(true);
            Node.LoadFiles(true);

            List<Node> trees = await _context.Nodes.Where(q => q.NodeId == null && !q.IsDeleted).ToListAsync();

            return trees;
        }

        // GET: api/Nodes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Node>>> GetNodes()
        {
            Node.LoadFiles(false);
            Node.LoadChildren(false);
            return await _context.Nodes.Where(q => !q.IsDeleted).ToListAsync();
        }

        [HttpPut("ImageTest/{id}")]
        public async Task<ActionResult<string>> GetImageBase64Test(Guid id, Test test)
        {
            
            string base64 = Convert.ToBase64String(test.Image);
            return base64;
           

            return test.StringVar;
        }

        // GET: api/Nodes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Node>> GetNode(Guid id)
        {
            Node.LoadFiles(true);
            Node.LoadChildren(false);
            var node = await _context.Nodes.FindAsync(id);

            if (node == null)
            {
                return NotFound();
            }

            return node;
        }

        // PUT: api/Nodes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Node>> PutNode(Guid id, UpdateNode input)
        {   
            Node.LoadFiles(false);
            Node.LoadChildren(false);

            if (id != input.Id)
            {
                return BadRequest();
            }

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
                if (!NodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return node;
        }

        // POST: api/Nodes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Node>> PostNode(Node node)
        {
            Node.LoadChildren(false);
            _context.Nodes.Add(node);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(Node), new { id = node.Id }, node);
            return node;
        }

        // DELETE: api/Nodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNode(Guid id)
        {
            Node.LoadChildren(false);
            var node = await _context.Nodes.FindAsync(id);
            if (node == null)
            {
                return NotFound();
            }

            _context.Nodes.Remove(node);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NodeExists(Guid id)
        {
            Node.LoadChildren(false);
            return _context.Nodes.Any(e => e.Id == id);
        }

        private bool FileExists(Guid id)
        {
            return _context.Files.Any(e => e.Id == id);
        }
    }
}
