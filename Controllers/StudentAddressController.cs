using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAddressController : ControllerBase
    {
        private readonly StudentsContext _context;

        public StudentAddressController(StudentsContext context)
        {
            _context = context;
        }

        // GET: api/StudentAddress
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentAddress>>> GetStudentAddresses()
        {
          if (_context.StudentAddresses == null)
          {
              return NotFound();
          }
            return await _context.StudentAddresses.ToListAsync();
        }

        // GET: api/StudentAddress/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentAddress>> GetStudentAddress(int id)
        {
          if (_context.StudentAddresses == null)
          {
              return NotFound();
          }
            var studentAddress = await _context.StudentAddresses.FindAsync(id);

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

            _context.Entry(studentAddress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
          if (_context.StudentAddresses == null)
          {
              return Problem("Entity set 'StudentsContext.StudentAddresses'  is null.");
          }
            _context.StudentAddresses.Add(studentAddress);
            try
            {
                await _context.SaveChangesAsync();
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
            if (_context.StudentAddresses == null)
            {
                return NotFound();
            }
            var studentAddress = await _context.StudentAddresses.FindAsync(id);
            if (studentAddress == null)
            {
                return NotFound();
            }

            _context.StudentAddresses.Remove(studentAddress);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentAddressExists(int id)
        {
            return (_context.StudentAddresses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
