using Microsoft.EntityFrameworkCore;
using ContactAgentMicroservice.Models;

namespace ContactAgentMicroservice.Data
{
    public class ContactAgentContext : DbContext
    {
        public ContactAgentContext(DbContextOptions<ContactAgentContext> options)
            : base(options)
        {
        }

        public DbSet<ContactAgent> ContactAgents { get; set; }

        public async Task<ContactAgent> GetContactAgentByAgentEmail(string email)
        {
            return await ContactAgents
                .Where(ca => ca.Agent.Email == email)
                .FirstOrDefaultAsync();
        }
    }
}



