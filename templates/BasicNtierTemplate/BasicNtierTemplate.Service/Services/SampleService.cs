using AutoMapper;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Repository;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Service.Services
{
    public class SampleService : ISampleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SampleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Blog

        // 1. GET operations (queries)

        public IEnumerable<BlogDto> GetBlogList()
        {
            var blogs = _unitOfWork.BlogRepository.GetAll().Include(blogs => blogs.Posteos);
            return _mapper.Map<IEnumerable<BlogDto>>(blogs);
        }

        public BlogDto? GetBlogItem(int blogId)
        {
            var blog = _unitOfWork.BlogRepository.GetAll()
                .Where(blog => blog.id == blogId)
                .Include(blog => blog.Posteos)
                .FirstOrDefault()
                ;

            if (blog == null) return null;

            var blogDto = _mapper.Map<BlogDto>(blog);
            return blogDto;
        }

        // 2. SAVE operations (create/update)

        public int SaveBlog(BlogDto blogDto)
        {
            if (blogDto.Id == 0)
            {
                // Create new blog
                var blog = _mapper.Map<Blog>(blogDto);
                _unitOfWork.BlogRepository.Add(blog);
                _unitOfWork.Save();
                return blog.id;
            }
            else
            {
                // Update existing blog
                var existingBlog = _unitOfWork.BlogRepository.GetById(blogDto.Id);
                if (existingBlog == null) return 0;

                _mapper.Map(blogDto, existingBlog);
                _unitOfWork.BlogRepository.Update(existingBlog);
                _unitOfWork.Save();
                return existingBlog.id;
            }
        }

        // 3. DELETE operations

        public bool DeleteBlog(int blogId)
        {
            var blog = _unitOfWork.BlogRepository.GetById(blogId);
            if (blog == null) return false;

            _unitOfWork.BlogRepository.Delete(blog);
            _unitOfWork.Save();
            return true;
        }

        #endregion

        #region Post

        // 1. GET operations (queries)

        public IEnumerable<PostDto> GetPostList()
        {
            var posts = _unitOfWork.PostRepository.GetAll();
            return _mapper.Map<IEnumerable<PostDto>>(posts);
        }

        public PostDto? GetPostItem(int postId)
        {
            var post = _unitOfWork.PostRepository.GetById(postId);
            if (post == null) return null;

            var postDto = _mapper.Map<PostDto>(post);
            return postDto;
        }

        // 2. SAVE operations (create/update)

        public int SavePost(PostDto postDto)
        {
            if (postDto.Id == 0)
            {
                // Create new post
                var post = _mapper.Map<Posteo>(postDto);
                _unitOfWork.PostRepository.Add(post);
                _unitOfWork.Save();
                return post.id;
            }
            else
            {
                // Update existing post
                var existingPost = _unitOfWork.PostRepository.GetById(postDto.Id);
                if (existingPost == null) return 0;

                _mapper.Map(postDto, existingPost);
                _unitOfWork.PostRepository.Update(existingPost);
                _unitOfWork.Save();
                return existingPost.id;
            }
        }

        // 3. DELETE operations

        public bool DeletePost(int postId)
        {
            var post = _unitOfWork.PostRepository.GetById(postId);
            if (post == null) return false;

            _unitOfWork.PostRepository.Delete(post);
            _unitOfWork.Save();
            return true;
        }

        #endregion
    }
}
