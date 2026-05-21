using BasicNLayerTemplate.Infrastructure.Entities;

namespace BasicNLayerTemplate.Data.Model
{
    public partial class Course : IEntity
    {
        public object ID => CourseId;
    }
}
