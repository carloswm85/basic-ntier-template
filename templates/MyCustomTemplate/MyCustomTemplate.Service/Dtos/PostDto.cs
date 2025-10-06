namespace MyCustomTemplate.Service.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

        public int BlogId { get; set; }
        public BlogDto Blog { get; set; }
    }
}
