using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AgentMicroservice.Controllers;
using AgentMicroservice.Models;
using AgentMicroservice.Services;
using Microsoft.Extensions.Configuration;

namespace CapstoneTesting.AgentTesting
{
    public class AgentControllerTests
    {
        private Mock<IAgentService> _serviceMock;
        private AgentController _controller;

        [SetUp]
        public void SetUp()
        {
            _serviceMock = new Mock<IAgentService>();
            var configurationMock = new Mock<IConfiguration>();
            _controller = new AgentController(_serviceMock.Object, configurationMock.Object);
        }

       
        [Test]
        public async Task PostAgent_CreatesAgent_ReturnsCreated()
        {
         
            var agent = new Agent { Name = "New Agent" };
            _serviceMock.Setup(s => s.AddAgentAsync(agent)).Returns(Task.CompletedTask);

      
            var result = await _controller.PostAgent(agent);

          
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(agent, createdResult.Value);
        }

    
        [Test]
        public async Task DeleteAgent_ReturnsNoContent_WhenDeleted()
        {
          
            var agentName = "Agent To Delete";
            _serviceMock.Setup(s => s.DeleteAgentAsync(agentName)).Returns(Task.CompletedTask);

       
            var result = await _controller.DeleteAgent(agentName);

        
            Assert.IsInstanceOf<NoContentResult>(result);
        }

    
        [Test]
        public async Task GetAgent_ReturnsOk_WithAgent()
        {
     
            var agentName = "Existing Agent";
            var agent = new Agent { Name = agentName };
            _serviceMock.Setup(s => s.GetAgentByNameAsync(agentName)).ReturnsAsync(agent);

      
            var result = await _controller.GetAgent(agentName);

      
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(agent, okResult.Value);
        }

     
     
        [Test]
        public async Task GetAllAgents_ReturnsOk_WithAgents()
        {
      
            var agents = new List<Agent>
    {
        new Agent { Name = "Agent 1" },
        new Agent { Name = "Agent 2" }
    };
            _serviceMock.Setup(s => s.GetAllAgentsAsync()).ReturnsAsync(agents);

       
            var result = await _controller.GetAllAgents();

       
            var okResult = result.Result as OkObjectResult; 
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode); 
            Assert.AreEqual(agents, okResult.Value); 
        }

    }
}
