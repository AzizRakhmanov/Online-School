using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Dto.CourseDto
{
    public class CreatePostRequestForCourse
    {
        public string SubjectName { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public IdentityUser Teacher { get; set; }
    }
}
