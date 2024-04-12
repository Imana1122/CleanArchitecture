using Domain.StudentCRUD;
using Infrastructure.StudentCRUD;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.StudentCRUD.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpPost, Route("AddStudent")]
        public async Task<IActionResult> AddStudent(Student student)
        {
            var addStudent = await studentService.AddStudent(student);
            return Ok(addStudent);
        }

        [HttpGet, Route("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet, Route("GetStudentById")]
        public async Task<IActionResult> GetStudentById(String id)
        {
            var student = await studentService.GetStudentById(id);
            if (student == null)
            {
                // If the result is null, it means the student was not found
                return NotFound("Student not found");
            }

            // If the student was successfully updated, return Ok response
            return Ok(student);
        }

        [HttpPut, Route("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(Student student)
        {
            var result = await studentService.UpdateStudent(student);

            if (result == null)
            {
                // If the result is null, it means the student was not found
                return NotFound("Student not found");
            }

            // If the student was successfully updated, return Ok response
            return Ok(result);
        }



        [HttpDelete, Route("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            await studentService.DeleteStudent(id);
            
            return Ok();
        }
    }
}
