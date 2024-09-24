using System.ComponentModel.DataAnnotations;

namespace AgentMicroService.DTO
{
    public class LoginAgentDto
    {
        [Required]
       
        public string Mail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
