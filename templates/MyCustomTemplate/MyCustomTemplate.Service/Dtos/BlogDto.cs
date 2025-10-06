namespace MyCustomTemplate.Service.Dtos
{
    public partial class BlogDto
    {
        public int Id { get; set; }
        public required string Url { get; set; }

        public List<PostDto> Posts { get; set; } = new();
    }
}
