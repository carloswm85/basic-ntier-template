namespace BasicNtierTemplate.Data.Model
{
    public partial class Blog
    {
        public Guid id { get; set; }
        public required string url { get; set; }

        public virtual ICollection<Posteo>? Posteos { get; set; }
    }
}
