using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PropertyCatalogueService.Data;
using PropertyCatalogueService.Models;

namespace PropertyCatalogueService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add MongoDB context
            builder.Services.AddSingleton<PropertyContext>(); // Registering the MongoDB context

            // Register the repository and pass the image folder path
            builder.Services.AddScoped<IPropertyRepository>(provider =>
                new PropertyRepository(
                    provider.GetRequiredService<PropertyContext>(),
                    "images" // Specify the subdirectory for image uploads within wwwroot
                ));

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(); // Ensure CORS policy is applied

            app.UseAuthorization(); // Ensure authorization middleware is in place

            app.UseStaticFiles(); // To serve static files from wwwroot

            app.MapControllers(); // Map controller routes

            app.Run(); // Start the application
        }
    }
}
