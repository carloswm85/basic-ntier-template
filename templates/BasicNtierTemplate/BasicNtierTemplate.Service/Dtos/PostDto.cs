namespace BasicNtierTemplate.Service.Dtos
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

        public Guid BlogId { get; set; }
        public BlogDto? Blog { get; set; }
    }
}
