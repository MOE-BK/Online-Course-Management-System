using Online_Course_Management_System.Validation;
using System.ComponentModel.DataAnnotations;

namespace Online_Course_Management_System.ViewModel
{
    [CoursePriceDuration]
    public class CourseCreateViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Range(1, 500)]
        public int Duration { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue)]
        public int InstructorId { get; set; }

        public IFormFile? Image { get; set; }
        public object Id { get; internal set; }
    }
}