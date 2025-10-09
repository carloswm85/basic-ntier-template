namespace MyCustomTemplate.Data.Entities
{
    public partial class Blog
    {
        public int id { get; set; }
        public required string url { get; set; }

        public virtual ICollection<Posteo>? posteos { get; set; }
    }
}
