using Domain.StudentCRUD;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.StudentCRUD
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDBContext _dbContext;

        public StudentService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Student> AddStudent(Student student)
        {
            var result = await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteStudent(string id)
        {
            // Convert the provided string ID to a Guid
            Guid studentId = Guid.Parse(id);

            // Find the student by id
            var existingStudent = await _dbContext.Students.FindAsync(studentId);

            if (existingStudent != null)
            {
                // Remove the student from the DbSet
                _dbContext.Students.Remove(existingStudent);

                // Save changes to the database
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // If the student with given Id is not found, you might want to handle this case accordingly

                throw new InvalidOperationException("Student not found.");
            }
        }



        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetStudentById(string id)
        {
            var result = await _dbContext.Students.Where(s => s.Id.ToString() == id).ToListAsync();
            return result;
        }

        public async Task<Student?> UpdateStudent(Student student)
        {
            var existingStudent = await _dbContext.Students.FindAsync(student.Id);

            if (existingStudent != null)
            {
                // Update existing student entity with new values
                existingStudent.Name = student.Name;
                existingStudent.Email = student.Email;
                existingStudent.Gender = student.Gender;
                existingStudent.Phone = student.Phone;

                // Update other properties as needed

                await _dbContext.SaveChangesAsync();
                return existingStudent;
            }
            else
            {
                // If the student with given Id is not found, you might want to handle this case accordingly
                return null;
            }
        }
    }
}
