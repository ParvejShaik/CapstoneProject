using System.Collections.Generic;
using System.Threading.Tasks;
using ContactAgentMicroservice.Models;
using ContactAgentMicroservice.Repository;

namespace ContactAgentMicroservice.Services
{
    public class ContactAgentService : IContactAgentService
    {
        private readonly IContactAgentRepository _repository;

        public ContactAgentService(IContactAgentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ContactAgent>> GetAllContactAgentsAsync()
        {
            return await _repository.GetAllContactAgentsAsync();
        }

        public async Task<ContactAgent> GetContactAgentByIdAsync(int Id)
        {
            return await _repository.GetContactAgentByIdAsync(Id);
        }

        public async Task AddContactAgentAsync(ContactAgent contactAgent)
        {
            await _repository.AddContactAgentAsync(contactAgent);
        }

        public async Task<bool> DeleteContactAgentAsync(int Id)
        {
            return await _repository.DeleteContactAgentAsync(Id);
        }

        public async Task<int> GetContactAgentCountAsync()
        {
            return await _repository.GetContactAgentCountAsync();
        }

        public async Task<IEnumerable<ContactAgent>> GetContactAgentsByLocalityAsync(string locality)
        {
            return await _repository.GetContactAgentsByLocalityAsync(locality);
        }

        public async Task<IEnumerable<ContactAgent>> GetContactAgentByAgentEmail(string Email)
        {
            return await _repository.GetContactAgentByAgentEmail(Email);
        }
    }
}

