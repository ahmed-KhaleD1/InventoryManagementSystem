
using Hangfire;
using InventoryManagement.API.Configuration;
using InventoryManagement.Application.BackgroundJobs;

namespace InventoryManagement_Ropoost_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.ConfigureExtinction(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseHangfireDashboard();
            app.MapControllers();

            RecurringJob.AddOrUpdate<LowStockCheckJob>("low-stock-check", job => job.Execute(), Cron.Daily);
            RecurringJob.AddOrUpdate<TransactionArchiveJob>("transaction-archive", job => job.Execute(), Cron.Yearly);
            
            app.Run();
        }
    }
}
