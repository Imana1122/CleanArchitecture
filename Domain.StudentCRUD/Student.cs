using System.ComponentModel.DataAnnotations;

namespace Domain.StudentCRUD
{
    public class Student
    {
        [Key]

        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public string? Phone { get; set; }

    }
}
