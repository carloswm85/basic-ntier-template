namespace MyCustomTemplate.Data.Entities
{
    public partial class Post
    {
        public int PostId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }

        public int BlogId { get; set; }
    }
}
