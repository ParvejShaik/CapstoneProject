using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContactAgentMicroservice.Data;
using ContactAgentMicroservice.Models;

namespace ContactAgentMicroservice.Repository
{
    public class ContactAgentRepository : IContactAgentRepository
    {
        private readonly ContactAgentContext _context;

        public ContactAgentRepository(ContactAgentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ContactAgent>> GetAllContactAgentsAsync()
        {
            return await _context.ContactAgents.ToListAsync();
        }

        public async Task<ContactAgent> GetContactAgentByIdAsync(int Id)
        {
            return await _context.ContactAgents.FindAsync(Id);
        }

        public async Task AddContactAgentAsync(ContactAgent contactAgent)
        {
            _context.ContactAgents.Add(contactAgent);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteContactAgentAsync(int Id)
        {
            var contactAgent = await GetContactAgentByIdAsync(Id);
            if (contactAgent == null) return false;

            _context.ContactAgents.Remove(contactAgent);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetContactAgentCountAsync()
        {
            return await _context.ContactAgents.CountAsync();
        }

        public async Task<IEnumerable<ContactAgent>> GetContactAgentsByLocalityAsync(string locality)
        {
            return await _context.ContactAgents
                .Where(ca => ca.Locality == locality)
                .ToListAsync();
        }

        public async Task<IEnumerable<ContactAgent>> GetContactAgentByAgentEmail(string Email)
        {
            return await _context.ContactAgents
                .Where(ca => ca.Agent.Email==Email)
           .ToListAsync();
        }
    }
}
