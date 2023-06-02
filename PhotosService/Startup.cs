using Microsoft.AspNetCore.Authentication.Certificate;
using PhotosService.Services;
using PhotosService.Repository;

namespace PhotosService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IConfiguration _configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configure and register BlobStorageRepository
            services.AddSingleton<IBlobStorageRepository, BlobStorageRepository>((provider) =>
            {
                var cosmosConnectionString = _configuration.GetConnectionString("StorageAccount");
                var containerName = _configuration["StorageContainers:PhotoContainerName"];

                return new BlobStorageRepository(cosmosConnectionString, containerName);
            });

            services.AddSingleton<IPhotoService, PhotoService>();

            services.AddAuthentication(
                CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    // Configure the Swagger UI for each document
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Photo Service APIs");
                });
            }
            else
            {
                app.UseSwaggerUI(c =>
                {
                    // Configure the Swagger UI for each document
                    c.SwaggerEndpoint("/PhotosService/swagger/v1/swagger.json", "Photo Service APIs");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

        }
    }
}
