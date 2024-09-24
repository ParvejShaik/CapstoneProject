using Microsoft.AspNetCore.Mvc;
using PropertyCatalogueService.Data;
using PropertyCatalogueService.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PropertyCatalogueService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyRepository _repository;
        private readonly string _imageFolderPath;

        public PropertiesController(IPropertyRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), configuration["FileStorageSettings:ImageFolderPath"]);
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }
        [HttpGet("total-properties")]
        public async Task<ActionResult<int>> GetTotalProperties()
        {
            var count = await _repository.GetAllPropertiesCountAsync();
            return Ok(new { count });
        }

        // Get all properties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Property>>> GetAllProperties()
        {
            var properties = await _repository.GetAllPropertiesAsync();
            return Ok(properties);
        }

        // Get a property by its title
        [HttpGet("{title}")]
        public async Task<ActionResult<Property>> GetPropertyByTitle(string title)
        {
            var property = await _repository.GetPropertyByTitleAsync(title);
            if (property == null)
            {
                return NotFound();
            }
            return Ok(property);
        }

        // Get properties by status
       

        // Get properties by location
        [HttpGet("by-location/{location}")]
        public async Task<ActionResult<IEnumerable<Property>>> GetPropertiesByLocation(string location)
        {
            var properties = await _repository.GetPropertiesByLocationAsync(location);
            return Ok(properties);
        }

        // Create a new property
        [HttpPost]
        public async Task<ActionResult<Property>> CreateProperty([FromBody] Property property)
        {
            if (property == null)
            {
                return BadRequest("Property data is required.");
            }

            // Ensure image file name is handled correctly
            if (string.IsNullOrEmpty(property.Image) || !System.IO.File.Exists(Path.Combine(_imageFolderPath, property.Image)))
            {
                return BadRequest("Image file does not exist.");
            }

            await _repository.CreatePropertyAsync(property);
            return CreatedAtAction(nameof(GetPropertyByTitle), new { title = property.Title }, property);
        }

        // Update an existing property by title
        [HttpPut("{title}")]
        public async Task<IActionResult> UpdateProperty(string title, [FromBody] Property property)
        {
            if (property == null)
            {
                return BadRequest("Property data is required.");
            }

            if (title != property.Title)
            {
                return BadRequest("Title in URL and body must match.");
            }

            var existingProperty = await _repository.GetPropertyByTitleAsync(title);
            if (existingProperty == null)
            {
                return NotFound(new { message = $"Property with title '{title}' not found." });
            }

            // Ensure image file name is handled correctly
            if (!string.IsNullOrEmpty(property.Image) && !System.IO.File.Exists(Path.Combine(_imageFolderPath, property.Image)))
            {
                return BadRequest("Image file does not exist.");
            }

            property.PropId = existingProperty.PropId;

            var result = await _repository.UpdatePropertyByTitleAsync(property);
            if (!result)
            {
                return NotFound(new { message = $"Failed to update property with title '{title}'." });
            }

            return Ok("Property details updated successfully.");
        }

        // Delete a property by title
        [HttpDelete("{title}")]
        public async Task<IActionResult> DeleteProperty(string title)
        {
            var result = await _repository.DeletePropertyByTitleAsync(title);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // Get image by filename
        [HttpGet("images/{imageName}")]
        public IActionResult GetImage(string imageName)
        {
            var filePath = Path.Combine(_imageFolderPath, imageName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "image/jpeg"; // Determine the content type based on file extension if needed
            return File(fileBytes, contentType, imageName);
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage()
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var file = files[0]; // Assuming one file is uploaded
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Generate a unique file name

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0; // Reset the stream position
                var result = await _repository.UploadImageAsync(fileName, stream);

                return Ok(new { fileName = result });
            }
        }
    }
}
