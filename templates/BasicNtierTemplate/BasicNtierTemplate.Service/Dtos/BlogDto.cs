namespace BasicNtierTemplate.Service.Dtos
{
    public partial class BlogDto
    {
        public Guid Id { get; set; }
        public required string Url { get; set; }

        public List<PostDto>? Posts { get; set; }
    }
}
