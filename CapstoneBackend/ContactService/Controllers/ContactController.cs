using Microsoft.AspNetCore.Mvc;
using ContactAgentMicroservice.Data;
using ContactAgentMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ContactAgentMicroservice.Repository;

namespace ContactAgentMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactAgentController : ControllerBase
    {
        private readonly ContactAgentContext _context;
        private readonly ContactAgentRepository _repository;

        public ContactAgentController(ContactAgentContext context, ContactAgentRepository repository)
        {
            _context = context;
            _repository = repository;
        }
        // GET: api/contactagent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactAgent>>> GetAllContactAgents()
        {
            var contactAgents = await _context.ContactAgents
                .ToListAsync();
            return Ok(contactAgents);
        }

        // GET: api/contactagent/{id}
        [HttpGet("{Id}")]
        public async Task<ActionResult<ContactAgent>> GetContactAgent(int Id)
        {
            var contactAgent = await _context.ContactAgents
                .FirstOrDefaultAsync(ca => (ca.Id) == Id);

            if (contactAgent == null)
            {
                return NotFound();
            }
            return Ok(contactAgent);
        }

        // POST: api/contactagent
        [HttpPost]
        public async Task<ActionResult<ContactAgent>> PostContactAgent(ContactAgent contactAgent)
        {
            _context.ContactAgents.Add(contactAgent);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContactAgent), new { Id = (contactAgent.Id) }, contactAgent);
        }

    
   
    // DELETE: api/contactagent/{id}
    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteContactAgent(int Id)
    {
        var contactAgent = await _context.ContactAgents
            .FirstOrDefaultAsync(ca => (ca.Id) == Id);

        if (contactAgent == null)
        {
            return NotFound();
        }
        _context.ContactAgents.Remove(contactAgent);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/contactagent/count
    [HttpGet("count")]
    public async Task<ActionResult<int>> GetContactAgentCount()
    {
        var count = await _context.ContactAgents.CountAsync();
        return Ok(count);
    }

    // GET: api/contactagent/locality/{locality}
    [HttpGet("locality/{locality}")]
    public async Task<ActionResult<IEnumerable<ContactAgent>>> GetContactAgentsByLocality(string locality)
    {
        var contactAgents = await _context.ContactAgents
            .Where(ca => ca.Locality == locality)
            .ToListAsync();

        if (contactAgents.Count == 0)
        {
            return NotFound();
        }
        return Ok(contactAgents);
    }
        [HttpGet("GetByAgentEmail")]
        public async Task<IActionResult> GetContactAgentByAgentEmail([Required] string Email)
        {
            var contactAgent = await _repository.GetContactAgentByAgentEmail(Email);
            if (contactAgent == null)
            {
                return NotFound("No records found");
            }
            return Ok(contactAgent);
        }

        private bool ContactAgentExists(int Id)
    {
        return _context.ContactAgents.Any(ca => (ca.Id) == Id);
    }
}
}