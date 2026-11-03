using Microsoft.AspNetCore.Identity;

namespace Online_Course_Management_System.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}