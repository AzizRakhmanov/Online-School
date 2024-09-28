using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSchoolCrm.ViewModels.CourseModel
{
    public class CourseUpdateRequest
    {
        [DisplayName("Subject name")]
        [Required]
        public string SubjectName { get; set; }


        [DisplayName("Price")]
        [Required]
        public decimal Price { get; set; }

        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }

        [DisplayName("Teacher Id")]
        [Required]
        public string TeacherId { get; set; }
    }
}
