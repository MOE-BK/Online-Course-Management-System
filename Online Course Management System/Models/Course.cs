namespace Online_Course_Management_System.Models
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Duration { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public int InstructorId { get; set; }

        public Instructor Instructor { get; set; } = null!;
        public string? ImagePath { get; set; }
    }
}
