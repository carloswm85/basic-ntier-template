using Microsoft.AspNetCore.Mvc;
using MyCustomTemplate.Service.Dtos;
using MyCustomTemplate.Service.Services.Interfaces;

namespace MyCustomTemplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        // GET: api/Blogs
        [HttpGet]
        public ActionResult<IEnumerable<BlogDto>> GetBlogs()
        {
            var blogs = _blogService.GetBlogList();
            return Ok(blogs);
        }

        //// GET: api/Blogs/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Blog>> GetBlog(int id)
        //{
        //    var blog = await _context.Blog.FindAsync(id);

        //    if (blog == null)
        //    {
        //        return NotFound();
        //    }

        //    return blog;
        //}

        //// PUT: api/Blogs/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutBlog(int id, Blog blog)
        //{
        //    if (id != blog.BlogId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(blog).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!BlogExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Blogs
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Blog>> PostBlog(Blog blog)
        //{
        //    _context.Blog.Add(blog);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetBlog", new { id = blog.BlogId }, blog);
        //}

        //// DELETE: api/Blogs/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteBlog(int id)
        //{
        //    var blog = await _context.Blog.FindAsync(id);
        //    if (blog == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Blog.Remove(blog);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool BlogExists(int id)
        //{
        //    return _context.Blog.Any(e => e.BlogId == id);
        //}
    }
}
