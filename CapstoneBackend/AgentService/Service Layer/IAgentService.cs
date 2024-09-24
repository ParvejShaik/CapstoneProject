using AgentMicroservice.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgentMicroservice.Services
{
    public interface IAgentService
    {
        Task<IEnumerable<Agent>> GetAllAgentsAsync();
        Task<Agent> GetAgentByEmailAsync(string email);
        Task<Agent> GetAgentByNameAsync(string Name);
        Task<IEnumerable<Agent>> GetAgentsByLocalityAsync(string locality);
        Task AddAgentAsync(Agent agent);
     
        Task DeleteAgentAsync(string Name);
        Task<int> GetAgentCountAsync();
    }
}
