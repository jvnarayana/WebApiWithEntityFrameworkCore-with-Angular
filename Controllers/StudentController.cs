using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using WebApplication1.Entities;
using WebApplication1.Repositories;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
         readonly IGenericRepository<Student> _studentRepository;
         private readonly IDistributedCache _cache;

        public StudentController(IGenericRepository<Student> studentRepository, IDistributedCache cache)
        {
            _studentRepository = studentRepository;
            _cache = cache;
        }

        // GET: api/Student
        [HttpGet]
            public async Task<ActionResult<List<Student>>> GetStudents()
            {
                Student student = new Student();
                string cacheKey = "StudentList";
                string cacheStudents = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cacheStudents))
                {
                    var students = JsonConvert.DeserializeObject<List<Student>>(cacheStudents);
                    return Ok(students);
                }

                var studentsList = await _studentRepository
                    .GetAll()
                    .Include(s => s.Address)
                    .ToListAsync();
                 
                var cacheStudentsOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)

                };
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(studentsList), cacheStudentsOptions);

                return Ok(studentsList);
            }

        // GET: api/Student/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var cacheKey = $"Student_{id}";
            string cacheStudent = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheStudent))
            {
                var studentFromCache = JsonSerializer.Deserialize<Student>(cacheStudent);
                return Ok(studentFromCache);
            }
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            var cacheStudentObject = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
            var serializeStudent = JsonSerializer.Serialize(student);
            await _cache.SetStringAsync(cacheKey, serializeStudent, cacheStudentObject);

            return student;
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            try
            {
                _studentRepository.UpdateAsync(student);
                string cacheKey = $"Student_{id}";
                await _cache.RemoveAsync(cacheKey);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            await _studentRepository.AddAsync(student);
            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            await _studentRepository.DeleteAsync(student);
            string cacheKey = $"Student_{id}";
            await _cache.RemoveAsync(cacheKey);
            return NoContent();
        }

        private bool StudentExists(int id)
        {
            var student = _studentRepository.GetByIdAsync(id);
            if (student.Id == null)
            {
                return false;
            }
            return true;
        }
    }
}
