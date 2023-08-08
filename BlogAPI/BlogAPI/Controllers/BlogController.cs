using Microsoft.AspNetCore.Mvc;
using BlogAPI.Data;
using BlogAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly AppDbContext _context;

    public BlogController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Post>> GetPosts()
    {
        var posts = _context.Posts.ToList();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public ActionResult<Post> GetPost(int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, Post updatedPost)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        post.Title = updatedPost.Title;
        post.Body = updatedPost.Body;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
