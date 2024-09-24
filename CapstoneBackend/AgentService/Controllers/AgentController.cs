using AgentMicroservice.Models;
using AgentMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgentMicroService.DTO;

namespace AgentMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _service;
        private readonly string _jwtKey;

        public AgentController(IAgentService service, IConfiguration configuration)
        {
            _service = service;
            _jwtKey = configuration["Jwt:Key"];
        }

        // GET: api/agent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAllAgents()
        {
            var agents = await _service.GetAllAgentsAsync();
            return Ok(agents);
        }

        // GET: api/agent/{name}
        [HttpGet("{name}")]
        public async Task<ActionResult<Agent>> GetAgent(string name)
        {
            var agent = await _service.GetAgentByNameAsync(name);
            if (agent == null)
            {
                return NotFound();
            }
            return Ok(agent);
        }

        // GET: api/agent/locality/{locality}
        [HttpGet("locality/{locality}")]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAgentsByLocality(string locality)
        {
            var agents = await _service.GetAgentsByLocalityAsync(locality);
            return Ok(agents);
        }

        // POST: api/agent
        [HttpPost]
        public async Task<ActionResult<Agent>> PostAgent(Agent agent)
        {
            await _service.AddAgentAsync(agent);
            return CreatedAtAction(nameof(GetAgent), new { name = agent.Name }, agent);
        }

     

        // DELETE: api/agent/{name}
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteAgent(string name)
        {
            await _service.DeleteAgentAsync(name);
            return NoContent();
        }

        // GET: api/agent/count
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetAgentCount()
        {
            var count = await _service.GetAgentCountAsync();
            return Ok(count);
        }

        // POST: api/agent/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAgentDto loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest(new { Message = "Login data is null" });
            }
            var agent = await _service.GetAgentByEmailAsync(loginDto.Mail);
            if (agent == null)
            {
                return NotFound(new { Message = "Agent not found" });
            }
            if (agent.Password != loginDto.Password)
            {
                return BadRequest(new { Message = "Invalid password" });
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, agent.Mail),
                    new Claim(ClaimTypes.Name, agent.Name)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new { Token = tokenString, Name = agent.Name });
        }

        // GET: api/agent/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<Agent>> GetAgentByEmail(string email)
        {
            var agent = await _service.GetAgentByEmailAsync(email);
            if (agent == null)
            {
                return NotFound();
            }
            return Ok(agent);
        }
    }
}


