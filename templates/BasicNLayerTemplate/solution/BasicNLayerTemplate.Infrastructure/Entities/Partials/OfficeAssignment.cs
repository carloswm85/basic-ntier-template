using BasicNLayerTemplate.Infrastructure.Entities;

namespace BasicNLayerTemplate.Data.Model
{
    public partial class OfficeAssignment : IEntity
    {
        public object ID => InstructorId;
    }
}
