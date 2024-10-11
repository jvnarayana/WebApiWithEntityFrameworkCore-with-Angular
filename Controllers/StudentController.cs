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
using WebApplication1.DTO;
using WebApplication1.Entities;
using WebApplication1.Repositories;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
         readonly IGenericRepository<Student?> _studentRepository;
         private readonly IDistributedCache _cache;

        public StudentController(IGenericRepository<Student?> studentRepository, IDistributedCache cache)
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
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)

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

            var student = await _studentRepository.GetByIdAsync(id, x=>x.Address);

            if (student == null)
            {
                return NotFound();
            }

            var cacheStudentObject = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
            var serializeStudent = JsonSerializer.Serialize(student);
            await _cache.SetStringAsync(cacheKey, serializeStudent, cacheStudentObject);

            return student;
        }

        // PUT: api/Student/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, StudentCreateDTO? studentDTO)
        {
            if (id != studentDTO.Id)
            {
                return BadRequest();
            }

           
            var student = new Student
            { 
                Id = studentDTO.Id,
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                AddressId = studentDTO.AddressId,
                City = studentDTO.City,
                Address = null
            };
            var studentCacheList = new List<Student>();
            var studentCacheRemove = new Student();
            try
            {
                var cacheKey = "StudentList";
                var studentCache = await _cache.GetStringAsync(cacheKey);
                if (studentCache != null)
                {
                    studentCacheList = JsonConvert.DeserializeObject<List<Student>>(studentCache);
                }
               
                if (studentCacheList != null && studentCacheList.Count > 0)
                {
                    studentCacheRemove = studentCacheList?.FirstOrDefault(x => x.Id == id);  
                    studentCacheList?.Remove(studentCacheRemove);
                }
                await _studentRepository.UpdateAsync(student);
                studentCacheRemove = await _studentRepository.GetByIdAsync(id, x=>x.Address);
                studentCacheList?.Add(studentCacheRemove);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                 AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                };
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(studentCacheList), cacheOptions);
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

            return Ok();
        }

        // POST: api/Student
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List<Student>>> PostStudent(StudentCreateDTO? studentDTO)
        {
            var student = new Student
            {
                FirstName = studentDTO.FirstName,
                LastName = studentDTO.LastName,
                AddressId = studentDTO.AddressId,
                City = studentDTO.City,
                Address = null
            };
            await _studentRepository.AddAsync(student);
            string cacheKey = "StudentList";
            await _cache.RemoveAsync(cacheKey);
            var studentsList = await _studentRepository
                .GetAll()
                .Include(s => s.Address)
                .ToListAsync();
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            };
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(studentsList), cacheOptions);
            return CreatedAtAction("GetStudents", studentsList);
        }

        // DELETE: api/Student/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            string cacheKey = "StudentList";
            string cacheStudents = await _cache.GetStringAsync(cacheKey);
            var studentCacheList = JsonConvert.DeserializeObject<List<Student>>(cacheStudents);
            var studentRemove = studentCacheList?.FirstOrDefault(x => x.Id == id);
            if (studentRemove == null)
            {
                return NotFound($"Unable to find this student ID {studentRemove.Id}");
            }
            var student = await _studentRepository.GetByIdAsync(id);
            await _studentRepository.DeleteAsync(student);
            studentCacheList.Remove(studentRemove);
            var cacheOptions = new DistributedCacheEntryOptions
            {
              AbsoluteExpirationRelativeToNow   = TimeSpan.FromHours(1)
            };
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(studentCacheList), cacheOptions);
            string individualCacheKey = $"Student_{id}";
            await _cache.RemoveAsync(individualCacheKey);
            return Ok(studentCacheList);
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
