using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using ContactAgentMicroservice.Controllers;
using ContactAgentMicroservice.Data;
using ContactAgentMicroservice.Models;

namespace ContactAgentMicroservice.Tests
{
    public class ContactAgentControllerTests
    {
        private ContactAgentContext _context;
        private ContactAgentController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ContactAgentContext>()
                .UseInMemoryDatabase(databaseName: "ContactAgentDatabase")
                .Options;

            _context = new ContactAgentContext(options);
            _controller = new ContactAgentController(_context, null); 
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted(); 
            _context.Dispose(); 
        }

     
        [Test]
        public async Task PostContactAgent_ReturnsCreatedAtActionResult()
        {
        
            var contactAgent = new ContactAgent
            {
                FullName = "John Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                PropertyName = "Dream House",
                Locality = "Downtown",
                Agent = new AgentDetails
                {
                    Name = "Agent Smith",
                    Email = "agent.smith@example.com",
                    Contact = "9876543210"
                }
            };

        
            var result = await _controller.PostContactAgent(contactAgent);

        
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(nameof(ContactAgentController.GetContactAgent), createdAtActionResult.ActionName);

    
            var dbContactAgent = await _context.ContactAgents.FindAsync(contactAgent.Id);
            Assert.IsNotNull(dbContactAgent);
            Assert.AreEqual(contactAgent.FullName, dbContactAgent.FullName);
        }



      

    }
}