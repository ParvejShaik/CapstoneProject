using MongoDB.Bson;
using MongoDB.Driver;
using PropertyCatalogueService.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PropertyCatalogueService.Data
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly PropertyContext _context;
        private readonly string _imageFolderPath;

        public PropertyRepository(PropertyContext context, string imageFolderPath)
        {
            _context = context;
            _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageFolderPath);
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        public async Task<int> GetAllPropertiesCountAsync()
        {
            return (int)await _context.Properties.CountDocumentsAsync(_ => true);
        }

        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await _context.Properties.Find(prop => true).ToListAsync();
        }

        public async Task<Property> GetPropertyByTitleAsync(string title)
        {
            return await _context.Properties.Find(prop => prop.Title == title).FirstOrDefaultAsync();
        }

      

        public async Task<IEnumerable<Property>> GetPropertiesByLocationAsync(string location)
        {
            return await _context.Properties.Find(prop => prop.Location.ToLower() == location.ToLower()).ToListAsync();
        }

        public async Task CreatePropertyAsync(Property property)
        {
            if (property.PropId == ObjectId.Empty)
            {
                property.PropId = ObjectId.GenerateNewId();
            }
            await _context.Properties.InsertOneAsync(property);
        }

        public async Task<bool> UpdatePropertyByTitleAsync(Property property)
        {
            var result = await _context.Properties.ReplaceOneAsync(
                prop => prop.PropId == property.PropId,
                property
            );
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeletePropertyByTitleAsync(string title)
        {
            var result = await _context.Properties.DeleteOneAsync(prop => prop.Title == title);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<string> UploadImageAsync(string fileName, Stream inputStream)
        {
            var filePath = Path.Combine(_imageFolderPath, fileName);
            using (var outputStream = new FileStream(filePath, FileMode.Create))
            {
                await inputStream.CopyToAsync(outputStream); // Copy inputStream to outputStream
            }
            return fileName; // Return the file name for reference
        }
    }
}
