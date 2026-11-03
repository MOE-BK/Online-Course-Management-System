using System.ComponentModel.DataAnnotations;

namespace Online_Course_Management_System.ViewModel
{
    public class CourseEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 10)]
        public string Description { get; set; }

        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        public decimal Price { get; set; }

        [Range(1, 500, ErrorMessage = "Duration must be between 1 and 500")]
        public int Duration { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select an instructor")]
        public int InstructorId { get; set; }

        public IFormFile? Image { get; set; }

        public string? ExistingImagePath { get; set; }
    }
}