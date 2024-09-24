using System.Collections.Generic;
using System.Threading.Tasks;
using AgentMicroservice.Models;
using AgentMicroservice.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AgentMicroservice.Services
{
    public class AgentService : IAgentService
    {
        private readonly IAgentRepository _repository;

        public AgentService(IAgentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Agent>> GetAllAgentsAsync()
        {
            return await _repository.GetAllAgentsAsync();

        }
        

        public async Task<Agent> GetAgentByNameAsync(string Name)
        {
            return await _repository.GetAgentByNameAsync(Name);
        }

        public async Task<IEnumerable<Agent>> GetAgentsByLocalityAsync(string locality)
        {
            return await _repository.GetAgentsByLocalityAsync(locality);
        }

        public async Task AddAgentAsync(Agent agent)
        {
            await _repository.AddAgentAsync(agent);
        }

      

        public async Task DeleteAgentAsync(string Name)
        {
            await _repository.DeleteAgentAsync(Name);
        }

        public async Task<int> GetAgentCountAsync()
        {
            return await _repository.GetAgentCountAsync();
        }

        public async Task<Agent> GetAgentByEmailAsync(string email)
        {
            return await _repository.GetAgentByEmailAsync(email);
        }
    }
}
