
using Hangfire;
using InventoryManagement.Application.CQRs.Products.Commands;
using InventoryManagement.Infrastructure.Data;
using InventoryManagement.Infrastructure.Notifications;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.API.Configuration
{
    public static class ExtinctionConfiguration
    {
        public static void ConfigureExtinction(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
            services.AddScoped<INotificationService, LogNotificationService>();
            services.AddHangfireServer();

    
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
        }
    }
}
