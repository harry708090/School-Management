namespace SchoolManagement.Api.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string StudentNumber { get; set; } = "";
        public int SchoolClassId { get; set; }
    }
}

