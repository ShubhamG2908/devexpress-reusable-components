using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using MVCDemoApp.Models;

using Newtonsoft.Json;

namespace MVCDemoApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "json/student.json");

        public StudentController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        private List<Student> GetStudentListFromCache()
        {
            List<Student> students;

            if (_memoryCache.TryGetValue("students", out students))
            {
                return students;
            }

            var jsonData = System.IO.File.ReadAllText(_filePath);
            students = JsonConvert.DeserializeObject<StudentDTO>(jsonData).Students;

            _memoryCache.Set("students", students, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return students;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            var result = DataSourceLoader.Load(GetStudentListFromCache(), loadOptions);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string values)
        {
            var student = JsonConvert.DeserializeObject<Student>(values);
            var students = GetStudentListFromCache();
            student.Id = students.Count + 1;
            students.Add(student);

            _memoryCache.Set("students", students, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] long key, [FromForm] string values)
        {
            var student = JsonConvert.DeserializeObject<Student>(values);
            var students = GetStudentListFromCache();
            var ExistingStudent = students.Find(s => s.Id == key);

            if (ExistingStudent == null)
            {
                return NotFound();
            }

            student.FirstName = !string.IsNullOrWhiteSpace(student.FirstName) ? student.FirstName : ExistingStudent.FirstName;
            student.LastName = !string.IsNullOrWhiteSpace(student.LastName) ? student.LastName : ExistingStudent.LastName;
            student.DOB = student.DOB == DateTime.MinValue ? student.DOB : ExistingStudent.DOB;
            student.Gender = !string.IsNullOrWhiteSpace(student.Gender) ? student.Gender : ExistingStudent.Gender;
            student.ClassroomId = student.ClassroomId > 0 ? student.ClassroomId : ExistingStudent.ClassroomId;
            student.EnrollmentDate = student.EnrollmentDate == DateTime.MinValue ? student.EnrollmentDate : ExistingStudent.EnrollmentDate;
            student.ContactNumber = !string.IsNullOrWhiteSpace(student.ContactNumber) ? student.ContactNumber : ExistingStudent.ContactNumber;
            student.Email = !string.IsNullOrWhiteSpace(student.Email) ? student.Email : ExistingStudent.Email;
            student.Address = !string.IsNullOrWhiteSpace(student.Address) ? student.Address : ExistingStudent.Address;

            students.Remove(ExistingStudent);
            students.Add(student);
            _memoryCache.Set("students", students, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] long key)
        {
            var students = GetStudentListFromCache();
            var student = students.Find(s => s.Id == key);

            if (student == null)
            {
                return NotFound();
            }
            students.Remove(student);
            _memoryCache.Set("students", students, new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });
            return NoContent();
        }
    }
}
