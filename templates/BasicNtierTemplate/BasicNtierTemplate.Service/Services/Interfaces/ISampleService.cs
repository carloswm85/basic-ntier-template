using BasicNtierTemplate.Service.Dtos;

namespace BasicNtierTemplate.Service.Services.Interfaces
{
    public interface ISampleService
    {
        // 1. GET operations (queries)
        // 2. SAVE operations (create/update)
        // 3. DELETE operations

        #region Blog

        IEnumerable<BlogDto> GetBlogList();
        BlogDto? GetBlogItem(Guid blogId);
        Guid? SaveBlog(BlogDto blogDto);
        bool DeleteBlog(Guid blogId);

        #endregion

        #region Post

        IEnumerable<PostDto> GetPostList();
        PostDto? GetPostItem(int postId);
        Guid? SavePost(PostDto postDto);
        bool DeletePost(Guid postId);

        #endregion
    }
}
