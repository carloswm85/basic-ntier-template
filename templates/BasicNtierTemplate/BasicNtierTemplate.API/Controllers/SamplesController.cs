using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        private readonly ISampleService _sampleService;

        public SamplesController(ISampleService sampleService)
        {
            _sampleService = sampleService;
        }

        #region Blog

        // GET: api/Blogs
        [HttpGet("blogs")]
        public ActionResult<IEnumerable<BlogDto>> GetBlogs()
        {
            var blogs = _sampleService.GetBlogList();
            return Ok(blogs);
        }

        // GET: api/Blogs/5
        [HttpGet("blogs/{id}")]
        public ActionResult<BlogDto> GetBlog(Guid id)
        {
            var blog = _sampleService.GetBlogItem(id);

            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blog);
        }

        // POST: api/Blogs
        [HttpPost("blogs")]
        public ActionResult<int> SaveBlog(BlogDto blogDto)
        {
            var blogId = _sampleService.SaveBlog(blogDto);

            if (blogDto.Id == Guid.Empty)
            {
                return CreatedAtAction(nameof(GetBlog), new { id = blogId }, blogId);
            }

            return Ok(blogId);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("blogs/{id}")]
        public ActionResult<bool> DeleteBlog(Guid id)
        {
            var result = _sampleService.DeleteBlog(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        #endregion

        #region Post

        // GET: api/Posts
        [HttpGet("posts")]
        public ActionResult<IEnumerable<PostDto>> GetPosts()
        {
            var posts = _sampleService.GetPostList();
            return Ok(posts);
        }

        // GET: api/Posts/5
        [HttpGet("posts/{id}")]
        public ActionResult<PostDto> GetPost(int id)
        {
            var post = _sampleService.GetPostItem(id);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // POST: api/Posts
        [HttpPost("posts")]
        public ActionResult<int> SavePost(PostDto postDto)
        {
            var postId = _sampleService.SavePost(postDto);

            if (postDto.Id == Guid.Empty)
            {
                return CreatedAtAction(nameof(GetPost), new { id = postId }, postId);
            }

            return Ok(postId);
        }

        // DELETE: api/Posts/5
        [HttpDelete("posts/{id}")]
        public ActionResult<bool> DeletePost(Guid id)
        {
            var result = _sampleService.DeletePost(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        #endregion
    }
}
