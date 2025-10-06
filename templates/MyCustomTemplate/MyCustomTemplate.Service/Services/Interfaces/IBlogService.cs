using MyCustomTemplate.Service.Dtos;

namespace MyCustomTemplate.Service.Services.Interfaces
{
    public interface IBlogService
    {
        IEnumerable<BlogDto> GetBlogList();
        BlogDto? GetBlogItem(int blogId);
        int SaveBlog(BlogDto blogDto);
        bool DeleteBlog(int blogId);

        IEnumerable<PostDto> GetPostList();
        PostDto? GetPostItem(int postId);
        int SavePost(PostDto postDto);
        bool DeletePost(int postId);
    }
}
