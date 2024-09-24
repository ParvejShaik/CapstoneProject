using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PropertyCatalogueService.Controllers;
using PropertyCatalogueService.Data;
using PropertyCatalogueService.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CapstoneTesting.PropertyCatalogueTesting
{
    public class PropertiesControllerTests
    {
        private Mock<IPropertyRepository> _mockRepo;
        private Mock<IConfiguration> _mockConfig;
        private PropertiesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IPropertyRepository>();
            _mockConfig = new Mock<IConfiguration>();

          
            _mockConfig.Setup(config => config["FileStorageSettings:ImageFolderPath"]).Returns("wwwroot/images");

         
            if (!Directory.Exists("wwwroot/images"))
            {
                Directory.CreateDirectory("wwwroot/images");
            }

            _controller = new PropertiesController(_mockRepo.Object, _mockConfig.Object);
        }

        [TearDown]
        public void TearDown()
        {
       
            var imageFolderPath = "wwwroot/images";
            if (Directory.Exists(imageFolderPath))
            {
                Directory.Delete(imageFolderPath, true);
            }
        }





        [Test]
        public async Task GetAllProperties_ReturnsOkWithPropertiesList()
        {
        
            _mockRepo.Setup(repo => repo.GetAllPropertiesAsync())
                     .ReturnsAsync(GetTestProperties());

        
            var result = await _controller.GetAllProperties();

       
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var properties = okResult.Value as IEnumerable<Property>;
            Assert.IsNotNull(properties);
            Assert.AreEqual(2, properties.Count());
        }

        [Test]
        public async Task GetPropertyByTitle_ReturnsOkWithProperty()
        {
     
            var title = "Test Property 1";
            _mockRepo.Setup(repo => repo.GetPropertyByTitleAsync(title))
                     .ReturnsAsync(new Property { Title = title });

       
            var result = await _controller.GetPropertyByTitle(title);

         
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var property = okResult.Value as Property;
            Assert.IsNotNull(property);
            Assert.AreEqual(title, property.Title);
        }

        [Test]
        public async Task GetPropertyByTitle_ReturnsNotFoundWhenPropertyDoesNotExist()
        {
         
            var title = "NonExistentTitle";
            _mockRepo.Setup(repo => repo.GetPropertyByTitleAsync(title))
                     .ReturnsAsync((Property)null);

          
            var result = await _controller.GetPropertyByTitle(title);

       
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task CreateProperty_ReturnsCreatedAtAction()
        {
            var newProperty = new Property { Title = "New Property", Price = 300000, Location = "Miami", Size = 1800, Image = "image.jpg" };
            _mockRepo.Setup(repo => repo.CreatePropertyAsync(It.IsAny<Property>()))
                     .Returns(Task.CompletedTask);

        
            var imagePath = Path.Combine("wwwroot/images", newProperty.Image);
            File.WriteAllText(imagePath, "dummy content");

       
            var result = await _controller.CreateProperty(newProperty);

      
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetPropertyByTitle", createdAtActionResult.ActionName);
            Assert.AreEqual(newProperty.Title, createdAtActionResult.RouteValues["title"]);

      
            File.Delete(imagePath);
        }

        [Test]
        public async Task UpdateProperty_ReturnsOkWhenUpdatedSuccessfully()
        {
            
            var title = "ExistingProperty";
            var updatedProperty = new Property { Title = title, Price = 350000, Location = "New Location" };

            _mockRepo.Setup(repo => repo.GetPropertyByTitleAsync(title))
                     .ReturnsAsync(new Property { Title = title });

            _mockRepo.Setup(repo => repo.UpdatePropertyByTitleAsync(updatedProperty))
                     .ReturnsAsync(true);

           
            var result = await _controller.UpdateProperty(title, updatedProperty);

           
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Property details updated successfully.", okResult.Value);
        }

        [Test]
        public async Task DeleteProperty_ReturnsNoContentWhenDeleted()
        {
            
            var title = "PropertyToDelete";
            _mockRepo.Setup(repo => repo.DeletePropertyByTitleAsync(title))
                     .ReturnsAsync(true);

          
            var result = await _controller.DeleteProperty(title);

       
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteProperty_ReturnsNotFoundWhenPropertyDoesNotExist()
        {
        
            var title = "NonExistentProperty";
            _mockRepo.Setup(repo => repo.DeletePropertyByTitleAsync(title))
                     .ReturnsAsync(false);

          
            var result = await _controller.DeleteProperty(title);

       
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        private IEnumerable<Property> GetTestProperties()
        {
            return new List<Property>
            {
                new Property { Title = "Test Property 1", Price = 250000, Location = "New York", Size = 1500, Status = "Available" },
                new Property { Title = "Test Property 2", Price = 400000, Location = "Miami", Size = 2000, Status = "Sold" }
            };
        }
    }
}
