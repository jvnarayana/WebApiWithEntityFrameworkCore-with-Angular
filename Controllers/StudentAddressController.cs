using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAddressController : ControllerBase
    {
        private readonly IGenericRepository<StudentAddress> _studentAddressRepository;

        public StudentAddressController(IGenericRepository<StudentAddress> studentAddressRepository)
        {
            _studentAddressRepository =studentAddressRepository;
        }

        // GET: api/StudentAddress
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentAddress>>> GetStudentAddresses()
        {
            return Ok(await _studentAddressRepository.GetAllAsync());
        }

        // GET: api/StudentAddress/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentAddress>> GetStudentAddress(int id)
        {
            var studentAddress = await _studentAddressRepository.GetByIdAsync(id);
            if (studentAddress == null)
            {
                return NotFound();
            }

            return studentAddress;
        }

        // PUT: api/StudentAddress/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentAddress(int id, StudentAddress studentAddress)
        {
            if (id != studentAddress.Id)
            {
                return BadRequest();
            }

            try
            {
                await _studentAddressRepository.UpdateAsync(studentAddress);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentAddressExists(id))
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

        // POST: api/StudentAddress
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentAddress>> PostStudentAddress(StudentAddress studentAddress)
        {
          if (studentAddress == null)
          {
              return Problem("Entity set 'StudentsContext.StudentAddresses'  is null.");
          }
          
            try
            {
                await _studentAddressRepository.AddAsync(studentAddress);
            }
            catch (DbUpdateException)
            {
                if (StudentAddressExists(studentAddress.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudentAddress", new { id = studentAddress.Id }, studentAddress);
        }

        // DELETE: api/StudentAddress/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentAddress(int id)
        {
            var studentAddress = await _studentAddressRepository.GetByIdAsync(id);
            if (studentAddress == null)
            {
                return NotFound();
            }

            await _studentAddressRepository.DeleteAsync(studentAddress);
            return NoContent();
        }

        private bool StudentAddressExists(int id)
        {
            var studentAddress = _studentAddressRepository.GetByIdAsync(id);
            return studentAddress != null;
        }
    }
}
