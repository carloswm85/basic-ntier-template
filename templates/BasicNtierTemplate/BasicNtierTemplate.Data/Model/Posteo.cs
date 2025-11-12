namespace BasicNtierTemplate.Data.Model
{
    public partial class Posteo
    {
        public Guid id { get; set; }
        public required string titulo { get; set; }
        public required string contenido { get; set; }

        public Guid blogid { get; set; }
        public virtual Blog? blog { get; set; }
    }
}
