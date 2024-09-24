using AgentMicroservice.Data;
using AgentMicroservice.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgentMicroservice.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly AgentContext _context;

        public AgentRepository(AgentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            return await _context.Agents.ToListAsync();
        }

        public async Task<Agent> GetAgentByNameAsync(string Name)
        {
            return await _context.Agents.FindAsync(Name);

        }
        public async Task<Agent> GetAgentByEmailAsync(string mail)
        {
            return await _context.Agents.FirstOrDefaultAsync(x => x.Mail == mail);
        }



        public async Task<IEnumerable<Agent>> GetAgentsByLocalityAsync(string locality)
        {
            return await _context.Agents
                .Where(a => a.Locality == locality)
                .ToListAsync();
        }

        public async Task AddAgentAsync(Agent agent)
        {
            _context.Agents.Add(agent);
            await _context.SaveChangesAsync();
        }

      

        public async Task DeleteAgentAsync(string Name)
        {
            var agent = await _context.Agents.FirstOrDefaultAsync(a => a.Name == Name);
            if (agent != null)
            {
                _context.Agents.Remove(agent);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetAgentCountAsync()
        {
            return await _context.Agents.CountAsync();
        }
    }
}
