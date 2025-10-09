using Microsoft.AspNetCore.Mvc;
using MyCustomTemplate.Service.Dtos;
using MyCustomTemplate.Service.Services.Interfaces;

namespace MyCustomTemplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly ISampleService _blogService;

        public SampleController(ISampleService blogService)
        {
            _blogService = blogService;
        }

        #region Blog

        // GET: api/Blogs
        [HttpGet("blogs")]
        public ActionResult<IEnumerable<BlogDto>> GetBlogs()
        {
            var blogs = _blogService.GetBlogList();
            return Ok(blogs);
        }

        // GET: api/Blogs/5
        [HttpGet("blogs/{id}")]
        public ActionResult<BlogDto> GetBlog(int id)
        {
            var blog = _blogService.GetBlogItem(id);

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
            var blogId = _blogService.SaveBlog(blogDto);

            if (blogDto.Id == 0)
            {
                return CreatedAtAction(nameof(GetBlog), new { id = blogId }, blogId);
            }

            return Ok(blogId);
        }

        // DELETE: api/Blogs/5
        [HttpDelete("blogs/{id}")]
        public ActionResult<bool> DeleteBlog(int id)
        {
            var result = _blogService.DeleteBlog(id);

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
            var posts = _blogService.GetPostList();
            return Ok(posts);
        }

        // GET: api/Posts/5
        [HttpGet("posts/{id}")]
        public ActionResult<PostDto> GetPost(int id)
        {
            var post = _blogService.GetPostItem(id);

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
            var postId = _blogService.SavePost(postDto);

            if (postDto.Id == 0)
            {
                return CreatedAtAction(nameof(GetPost), new { id = postId }, postId);
            }

            return Ok(postId);
        }

        // DELETE: api/Posts/5
        [HttpDelete("posts/{id}")]
        public ActionResult<bool> DeletePost(int id)
        {
            var result = _blogService.DeletePost(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        #endregion
    }
}
