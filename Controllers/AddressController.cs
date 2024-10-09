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
    public class AddressController : ControllerBase
    {
        private readonly IGenericRepository<Address?> _addressRepository;

        public AddressController(IGenericRepository<Address?> addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // GET: api/Address
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            return Ok(await _addressRepository.GetAllAsync());
        }

        // GET: api/Address/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var address = await _addressRepository.GetByIdAsync(id);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        // PUT: api/Address/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(int id, Address? address)
        {
            if (id != address.AddressId)
            {
                return BadRequest();
            }

            try
            {
                _addressRepository.UpdateAsync(address);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        // POST: api/Address
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(Address? address)
        {
            await _addressRepository.AddAsync(address);
            return CreatedAtAction("GetAddress", new { id = address.AddressId }, address);
        }

        // DELETE: api/Address/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            if (_addressRepository.GetByIdAsync(id) == null)
            {
                return NotFound();
            }
            var student = await _addressRepository.GetByIdAsync(id);
            await _addressRepository.DeleteAsync(student);
            return NoContent();
        }

        private bool AddressExists(int id)
        {
            var address =  _addressRepository.GetByIdAsync(id);
            return address != null;
        }
    }
}
