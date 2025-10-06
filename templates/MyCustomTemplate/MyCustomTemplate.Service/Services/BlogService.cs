using MyCustomTemplate.Repository;
using MyCustomTemplate.Service.Dtos;
using MyCustomTemplate.Service.Services.Interfaces;

namespace MyCustomTemplate.Service.Services
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool DeleteBlog(int blogId)
        {
            throw new NotImplementedException();
        }

        public bool DeletePost(int postId)
        {
            throw new NotImplementedException();
        }

        public BlogDto? GetBlogItem(int blogId)
        {
            //var blog = _unitOfWork.BlogRepository.GetById(blogId);
            throw new NotImplementedException();
        }

        public IEnumerable<BlogDto> GetBlogList()
        {
            var blogs = _unitOfWork.BlogRepository.GetAll();

            return blogs.Select(static b => new BlogDto
            {
                Id = b.BlogId,
                Url = b.Url,
                Posts = b.Posts.Select(p => new PostDto
                {
                    Id = p.PostId,
                    Title = p.Title,
                    Content = p.Content
                }).ToList()
            }).ToList();
        }

        public PostDto? GetPostItem(int postId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PostDto> GetPostList()
        {
            throw new NotImplementedException();
        }

        public int SaveBlog(BlogDto blogDto)
        {
            throw new NotImplementedException();
        }

        public int SavePost(PostDto postDto)
        {
            throw new NotImplementedException();
        }
    }
}
