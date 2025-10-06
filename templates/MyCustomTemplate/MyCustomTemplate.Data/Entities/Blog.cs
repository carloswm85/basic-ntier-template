namespace MyCustomTemplate.Data.Entities
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public required string Url { get; set; }

        public List<Post> Posts { get; } = new();
    }
}
