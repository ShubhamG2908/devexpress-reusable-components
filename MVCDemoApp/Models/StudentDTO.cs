using System.ComponentModel.DataAnnotations;

namespace MVCDemoApp.Models
{
    public class StudentDTO
    {
        public List<Student> Students { get; set; }
    }

    public class Student
    {
        [Display(AutoGenerateField = false)]
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DOB { get; set; }
        public string? Gender { get; set; }
        public long ClassroomId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
