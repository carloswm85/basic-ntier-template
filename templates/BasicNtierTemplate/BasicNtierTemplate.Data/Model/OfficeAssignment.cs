using System.ComponentModel.DataAnnotations;

namespace BasicNtierTemplate.Data.Model
{
    public partial class OfficeAssignment
    {
        /* You could put a [Required] attribute on the Instructor navigation
         * property to specify that there must be a related instructor, but
         * you don't have to do that because the InstructorID foreign key
         * (which is also the key to this table) is non-nullable.
         */
        [Key]
        public int InstructorId { get; set; }

        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; } = string.Empty;

        public Instructor Instructor { get; set; } = default!;
    }
}
