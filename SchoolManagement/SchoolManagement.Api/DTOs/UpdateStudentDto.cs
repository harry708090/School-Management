using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Api.DTOs
{
    public class UpdateStudentDto
    {
        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        public string StudentNumber { get; set; } = "";

        [Required]
        public int SchoolClassId { get; set; }
    }
}