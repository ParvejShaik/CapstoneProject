using NUnit.Framework;
using UserManagementService.Controllers;
using UserManagementService.Data;
using UserManagementService.DTO;
using UserManagementService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace AllMicroservice_Tests.AuthMicroserviceTests
{
    [TestFixture]
    public class AuthControllerTests : IDisposable
    {
        private AuthController _controller;
        private ApplicationDbContext _context;
        private IConfiguration _mockConfig;

        public void Dispose()
        {
            
            _context?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
          
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"AuthMicroServiceTestsDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);

            
            _context.Users.AddRange(new List<User>
            {
                new User
                {
                    UserName = "example",
                    Password = "example@123", 
                    Email = "user@example.com",
                    Role = "User",
                    PhoneNumber = "1234567890"
                }
            });
            _context.SaveChanges();

         
            _mockConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:Key", "yT8kF3pLz9VbN6wQeR1jXaG5mPsH4oUv" }
                })
                .Build();

            _controller = new AuthController(_context, _mockConfig);
        }

      
        [Test]
        public async Task RegisterUser_NullUserDto_ReturnsBadRequest()
        {
            var result = await _controller.RegisterUser(null);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

    
        [Test]
        public async Task RegisterUser_InvalidRole_ReturnsBadRequest()
        {
            var userDto = new RegisterUserDto
            {
                UserName = "testuser",
                Email = "test@test.com",
                Password = "Password1!",
                Role = "InvalidRole",
                PhoneNumber = "9876543244"
            };
            var result = await _controller.RegisterUser(userDto);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

     
        [Test]
        public async Task Login_InvalidCredentials_ReturnsNotFound()
        {
            var loginDto = new LoginUserDto { Email = "invalid@test.com", Password = "WrongPassword" };
            var result = await _controller.Authenticate(loginDto);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

      
      

       
        [Test]
        public async Task GetUser_UserExists_ReturnsOkResult()
        {
            var result = await _controller.GetUser("example");

            var actionResult = result as ActionResult<User>;
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOf<OkObjectResult>(actionResult.Result);
        }

    
        [Test]
        public async Task UpdateUser_ValidData_ReturnsOkResult()
        {
            var updateDto = new UpdateUserDto { UserName = "prakash", PhoneNumber = "7993975200" };

            var result = await _controller.UpdateUser("example", updateDto);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

   
        [Test]
        public async Task UpdateUser_NonExistentUser_ReturnsNotFound()
        {
            var updateDto = new UpdateUserDto { UserName = "nonexistent", PhoneNumber = "0000000000" };

            var result = await _controller.UpdateUser("nonexistent", updateDto);
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

    
        [Test]
        public async Task RegisterUser_MissingRequiredFields_ReturnsBadRequest()
        {
            var incompleteUserDto = new RegisterUserDto
            {
                UserName = "incompleteuser",
        
            };

            var result = await _controller.RegisterUser(incompleteUserDto);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}