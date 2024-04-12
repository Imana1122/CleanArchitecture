using Domain.StudentCRUD;

namespace Infrastructure.StudentCRUD
{
    public interface IStudentService
    {
        Task<Student> AddStudent(Student staff);
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student?> UpdateStudent(Student staff);
        Task DeleteStudent(string id);
        Task<IEnumerable<Student>> GetStudentById(string id);
    }
}
