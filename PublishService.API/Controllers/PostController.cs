﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublishService.API.Data;
using PublishService.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PublishService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController:ControllerBase
    {
        private readonly PostServiceContext _context;
        public PostController(PostServiceContext context)
        {
            _context= context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost()
        {
            return await _context.Post.Include(x => x.User).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Post.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        } 
    }
}
