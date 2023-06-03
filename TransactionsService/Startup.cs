using AutoMapper;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.Azure.Cosmos;
using System.Runtime.CompilerServices;
using TransactionsService.Mapper;
using TransactionsService.ServiceBus;
using TransactionsService.Services;

namespace TransactionsService
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
            // Add CosmosClient instance
            services.AddSingleton((provider) =>
            {
                var cosmosConnectionString = _configuration.GetConnectionString("CosmosDB");
                var cosmosClientOptions = new CosmosClientOptions
                {
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                };

                return new CosmosClient(cosmosConnectionString, cosmosClientOptions);
            });

            // Configure and register TransactionRepository
            services.AddSingleton<ITransactionRepository, TransactionRepository>((provider) =>
            {
                var cosmosClient = provider.GetRequiredService<CosmosClient>();
                var databaseName = _configuration["CosmosDB:DatabaseName"];
                var containerName = _configuration["CosmosDB:ContainerName"];


                return new TransactionRepository(cosmosClient, databaseName, containerName);
            });

            // register services to DI
            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<IServiceBusListener, ServiceBusListener>();
            services.AddSingleton<IServiceBusPublisher, ServiceBusPublisher>();

            services.AddAuthentication(
                CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    // Configure the Swagger UI for each document - development
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transaction Service APIs");
                });
            }
            else
            {
                app.UseSwaggerUI(c =>
                {
                    // Configure the Swagger UI for each document - production
                    c.SwaggerEndpoint("/TransactionService/swagger/v1/swagger.json", "Transaction Service APIs");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.StartServiceBusListener();

        }
    }

    public static class AppExtension
    {
        // Start service bus listener
        public static void StartServiceBusListener(this IApplicationBuilder app)
        {
            var sb = app.ApplicationServices.GetService<IServiceBusListener>();
            sb.StartServiceBusReceiver(CancellationToken.None);

        }
    }
}
