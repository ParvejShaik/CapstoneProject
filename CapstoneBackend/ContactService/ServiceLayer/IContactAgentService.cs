using System.Collections.Generic;
using System.Threading.Tasks;
using ContactAgentMicroservice.Models;

namespace ContactAgentMicroservice.Services
{
    public interface IContactAgentService
    {
        Task<IEnumerable<ContactAgent>> GetAllContactAgentsAsync();
        Task<ContactAgent> GetContactAgentByIdAsync(int Id);
        Task AddContactAgentAsync(ContactAgent contactAgent);
        Task<bool> DeleteContactAgentAsync(int Id);
        Task<int> GetContactAgentCountAsync();
        Task<IEnumerable<ContactAgent>> GetContactAgentsByLocalityAsync(string locality);
        Task<IEnumerable<ContactAgent>> GetContactAgentByAgentEmail(string Email);
    }
}
