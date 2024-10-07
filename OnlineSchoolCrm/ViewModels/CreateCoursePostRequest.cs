using Microsoft.AspNetCore.Identity;

namespace OnlineSchoolCrm.ViewModels
{
    public class CreateCoursePostRequest
    {
        public string SubjectName { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string TeacherId { get; set; }
    }
}
