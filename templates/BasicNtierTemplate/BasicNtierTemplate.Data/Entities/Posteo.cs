namespace BasicNtierTemplate.Data.Entities
{
    public partial class Posteo
    {
        public int id { get; set; }
        public required string titulo { get; set; }
        public required string contenido { get; set; }

        public int blogid { get; set; }
        public virtual Blog? blog { get; set; }
    }
}
